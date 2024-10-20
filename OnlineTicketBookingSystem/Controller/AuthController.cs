using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;
using OnlineTicketBookingSystem.Models.DTO;
using OnlineTicketBookingSystem.Utility;
using System.Security.Claims;

namespace OnlineTicketBookingSystem.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;
        private readonly JwtService _jwtService;

        public AuthController(IUnitOfWork unitOfWork, EmailService emailService, JwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _jwtService = jwtService;
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return NotFound(new { code = 404, message = "Không tìm thấy thông tin người dùng" });
                }

                var isExistUser = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == userEmail);

                if (isExistUser != null)
                {
                    isExistUser.IsStatus = false;
                    _unitOfWork.User.Update(isExistUser);
                    await _unitOfWork.SaveAsync();

                    return Ok(new { code = 200, message = "Đăng xuất thành công" });
                }
                else
                {
                    return NotFound(new { code = 404, message = "Tài khoản không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = "Đã xảy ra lỗi ở server: " + ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                // Kiểm tra dữ liệu nhập
                if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
                {
                    return BadRequest(new { code = 400, message = "Email và mật khẩu không được để trống" });
                }

                var isExistUser = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == loginDto.Email);
                if (isExistUser != null)
                {
                    var isPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, isExistUser.Password);
                    if (isPassword)
                    {
                        if (!isExistUser.IsActive)
                        {
                            return Ok(new { code = 403, message = "Tài khoản chưa được kích hoạt" });
                        }
                        else
                        {
                            var token = _jwtService.GenerateToken(isExistUser);
                            isExistUser.IsStatus = SD.IsStatus_True;
                            isExistUser.LoginAttempts += 1;
                            isExistUser.LastLogin = DateTime.Now.ToString();
                            _unitOfWork.User.Update(isExistUser);
                            await _unitOfWork.SaveAsync();
                            return Ok(new { code = 200, message = "Đăng nhập thành công", token = token, fullName = isExistUser.FullName, group = isExistUser.GroupId, avatar = isExistUser.Avatar });
                        }
                    }
                    else
                    {
                        return NotFound(new { code = 404, message = "Tài khoản hoặc mật khẩu không hợp lệ" });
                    }
                }
                else
                {
                    return NotFound(new { code = 404, message = "Tài khoản hoặc mật khẩu không hợp lệ" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = "Đã xảy ra lỗi ở server: " + ex.Message });
            }
        }
        [HttpPost("CheckActivation")]
        public async Task<IActionResult> CheckActivation([FromBody] User user)
        {
            try
            {
                var isExistUser = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == user.Email);

                if (isExistUser != null && !isExistUser.IsActive)
                {
                    string codeId = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
                    DateTime codeExpired = DateTime.Now.AddSeconds(120);

                    await _emailService.SendVerificationEmailAsync(user.Email, isExistUser.FullName, "Xác thực tài khoản", codeId);

                    isExistUser.CodeId = codeId;
                    isExistUser.CodeExpired = codeExpired;
                    _unitOfWork.User.Update(isExistUser);
                    await _unitOfWork.SaveAsync();
                    return Ok(new { code = 200, message = "Mã xác thực đã được gửi đến email thành công!" });
                }
                else
                {
                    return NotFound(new { code = 404, message = "Tài khoản đã được kích hoạt hoặc không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = "Đã xảy ra lỗi ở server: " + ex.Message });
            }
        }

        [HttpPost("VerifyCode")]
        public async Task<IActionResult> VerifyCode([FromBody] User user)
        {
            try
            {
                var isExistUser = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == user.Email);
                if (isExistUser != null)
                {
                    if (isExistUser.CodeId == user.CodeId && isExistUser.CodeExpired > DateTime.Now)
                    {
                        isExistUser.IsActive = SD.IsActive_True;
                        isExistUser.CodeId = null;
                        isExistUser.CodeExpired = null;
                        _unitOfWork.User.Update(isExistUser);
                        await _unitOfWork.SaveAsync();
                        return Ok(new { code = 200, message = "Kích hoạt tài khoản thành công!" });
                    }
                    else if (isExistUser.CodeExpired <= DateTime.Now)
                    {
                        return NotFound(new { code = 404, message = "Mã xác thực đã hết hạn. Vui lòng gửi lại mã." });
                    }
                    else
                    {
                        return NotFound(new { code = 404, message = "Mã xác thực không hợp lệ." });
                    }
                }
                else
                {
                    return NotFound(new { code = 404, message = "Không tìm thấy người dùng!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = "Đã xảy ra lỗi ở server: " + ex.Message });
            }
        }
    }
}

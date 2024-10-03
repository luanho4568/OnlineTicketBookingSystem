using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;
using OnlineTicketBookingSystem.Utility;

namespace AdminDriverDashboard.Areas.Account.Controllers
{
    [Area("Account")]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;
        public AuthController(IUnitOfWork unitOfWork, EmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { code = 400, message = "Dữ liệu không được để trống" });
                var isExistUser = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == user.Email);
                if (isExistUser != null)
                {
                    var isPassword = BCrypt.Net.BCrypt.Verify(user.Password, isExistUser.Password);
                    if (isPassword)
                    {
                        if (!isExistUser.IsActive)
                        {
                            return Json(new { code = 403, message = "Tài khoản chưa được kích hoạt" });
                        }
                        else
                        {
                            isExistUser.IsStatus = SD.IsStatus_True;
                            isExistUser.LoginAttempts += 1;
                            isExistUser.LastLogin = DateTime.Now.ToString();
                            _unitOfWork.User.Update(isExistUser);
                            await _unitOfWork.SaveAsync();
                            return Ok(new { code = 200, message = "Đăng nhập thành công" });
                        }
                    }
                    else
                    {
                        return Json(new { code = 404, message = "Tài khoản hoặc mật khẩu không hợp lệ" });
                    }
                }
                else
                {
                    return Json(new { code = 404, message = "Tài khoản hoặc mật khẩu không hợp lệ" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = "Đã xảy ra lỗi ở server: " + ex.Message });
            }
        }
        [HttpPost]
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
                    return BadRequest(new { code = 404, message = "Tài khoản đã được kích hoạt hoặc không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = "Đã xảy ra lỗi ở server: " + ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> VerifyCode([FromBody] User user)
        {
            try
            {
                var isExistUser = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == user.Email);
                if (isExistUser != null)
                {
                    if (isExistUser.CodeId == user.CodeId && isExistUser.CodeExpired > DateTime.Now)
                    {
                        isExistUser.IsActive = true;
                        isExistUser.CodeId = null;
                        isExistUser.CodeExpired = null;
                        _unitOfWork.User.Update(isExistUser);
                        await _unitOfWork.SaveAsync();
                        return Json(new { code = 200, message = "Kích hoạt tài khoản thành công!" });
                    }
                    else if (isExistUser.CodeExpired <= DateTime.Now)
                    {
                        return Json(new { code = 404, message = "Mã xác thực đã hết hạn. Vui lòng gửi lại mã." });
                    }
                    else
                    {
                        return Json(new { code = 404, message = "Mã xác thực không hợp lệ." });
                    }
                }
                else
                {
                    return Json(new { code = 404, message = "Không tìm thấy người dùng!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = "Đã xảy ra lỗi ở server: " + ex.Message });
            }
        }

    }
}

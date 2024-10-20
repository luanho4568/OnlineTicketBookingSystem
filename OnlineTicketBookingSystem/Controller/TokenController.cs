using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Utility;
using System.Security.Claims;

namespace OnlineTicketBookingSystem.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public TokenController(JwtService jwtService, IUnitOfWork unitOfWork)
        {
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("ValidateAndDecodeToken")]
        public async Task<IActionResult> ValidateAndDecodeTokenAsync([FromBody] string token)
        {
            try
            {
                var claims = _jwtService.ValidateAndDecodeToken(token);

                if (claims.Count == 0)
                {
                    return NotFound(new { code = 404, message = "Token không hợp lệ hoặc không có thông tin" });
                }

                return Ok(new { code = 200, message = "Giải mã thành công", data = claims });
            }
            catch (SecurityTokenExpiredException)
            {
                var claims = _jwtService.ValidateAndDecodeToken(token);
                if (claims.TryGetValue(ClaimTypes.NameIdentifier, out string nameid))
                {
                    // Lấy thông tin người dùng từ cơ sở dữ liệu
                    var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Id.ToString() == nameid);

                    if (user != null)
                    {
                        user.IsStatus = false;
                        _unitOfWork.User.Update(user);
                        await _unitOfWork.SaveAsync();
                    }
                }
                return Unauthorized(new { code = 401, message = "Token đã hết hạn" });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { code = 401, message = "Token không hợp lệ: " + ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }
    }
}

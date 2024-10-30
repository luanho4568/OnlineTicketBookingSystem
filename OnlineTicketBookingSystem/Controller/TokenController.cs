using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Utility;

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
            catch (SecurityTokenException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized(new { code = 401, message = "Phiên bản đăng nhập đã hết hạn" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = ex.Message });
            }
        }
    }
}

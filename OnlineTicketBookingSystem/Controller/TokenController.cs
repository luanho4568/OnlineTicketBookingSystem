using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.Utility;

namespace OnlineTicketBookingSystem.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public TokenController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }
        [HttpPost("DecodeToken")]
        public IActionResult DecodeToken([FromBody] string token)
        {
            try
            {
                var claims = _jwtService.DecodeToken(token);

                if (claims.Count == 0)
                {
                    return NotFound(new { code = 404, message = "Token không hợp lệ hoặc không có thông tin" });
                }

                return Ok(new { code = 200, message = "Giải mã thành công", data = claims });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }
    }
}

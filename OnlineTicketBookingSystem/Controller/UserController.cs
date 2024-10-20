using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Utility;

namespace OnlineTicketBookingSystem.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _env;
        private readonly JwtService _jwtService;

        public UserController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }
        [HttpGet("GetUserByGroup")]
        public async Task<IActionResult> GetUserByGroup(int groupId)
        {
            try
            {
                var users = await _unitOfWork.User.GetFirstOrDefaultAsync(u => u.GroupId == groupId, includeProperties: "Province,District,Ward");
                if (users == null) return NotFound(new { code = 404, message = "Lấy user thất bại!" });
                return Ok(new { code = 200, message = "Lấy user thành công!", data = users });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
            }
        }
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAll(int groupId)
        {
            try
            {
                var users = await _unitOfWork.User.GetAllAsync(u => u.GroupId == groupId, includeProperties: "Province,District,Ward");
                if (users == null || !users.Any()) return Ok(new { code = 404, message = "Lấy danh sách user thất bại!", data = users });
                return Ok(new { code = 200, message = "Lấy danh sách user thành công!", data = users });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {
                var users = await _unitOfWork.User.GetFirstOrDefaultAsync(u => u.Id == id);
                if (users == null) return NotFound(new { code = 404, message = "Không tìm thấy user!" });
                await _unitOfWork.User.RemoveAsync(users);
                await _unitOfWork.SaveAsync();
                return Ok(new { code = 200, message = "Xoá thành công!" });
            }
            catch (Exception e)
            {

                return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
            }
        }
        [HttpPost("Update-Avatar")]
        public async Task<IActionResult> UpdateUserProfileAvatar([FromBody] string fileName)
        {
            try
            {
                // Kiểm tra xem tên tệp có hợp lệ hay không
                if (string.IsNullOrEmpty(fileName))
                {
                    return BadRequest(new { code = 400, message = "Tên file không hợp lệ" });
                }

                string wwwRootPath = _env.WebRootPath;
                var avatarUrl = Path.Combine(wwwRootPath, "images/drivers", fileName);

                // Kiểm tra xem tệp có tồn tại hay không
                if (System.IO.File.Exists(avatarUrl))
                {
                    string relativePath = Path.Combine("images/drivers", fileName).Replace("\\", "/"); // Thay thế \ bằng /

                    return Ok(new { code = 200, message = "Avatar đã tồn tại", avatarUrl = relativePath });
                }
                else
                {
                    return NotFound(new { code = 401, message = "Không tìm thấy Avatar", avatarUrl });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
            }
        }

    }
}

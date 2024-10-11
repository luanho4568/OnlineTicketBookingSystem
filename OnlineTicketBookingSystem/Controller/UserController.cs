using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;

namespace OnlineTicketBookingSystem.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}

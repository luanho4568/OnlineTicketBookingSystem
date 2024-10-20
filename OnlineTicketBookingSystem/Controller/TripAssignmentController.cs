using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models.DTO;

namespace OnlineTicketBookingSystem.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TripAssignmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TripAssignmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetAllTripAssignments")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                await _unitOfWork.Trips.GetAllAsync(includeProperties: "Buses,StartProvince,EndProvince");
                var tripAssignments = await _unitOfWork.TripsAssignments.GetAllAsync(includeProperties: "Trips,Driver");
                if (tripAssignments == null || !tripAssignments.Any()) return Ok(new { code = 404, message = "Lấy danh sách phê duyệt thất bại!", data = tripAssignments });
                return Ok(new { code = 200, message = "Lấy danh sách phê duyệt thành công!", data = tripAssignments });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
            }
        }
        [HttpPost("RegisterTrip")]
        public async Task<IActionResult> RegisterTrip([FromBody] TripAssignmentDTO tripAssignmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { code = 400, message = "Dữ liệu không hợp lệ" });
            }

            try
            {
                // Kiểm tra xem TripId và DriverId có hợp lệ không
                var trip = await _unitOfWork.Trips.GetFirstOrDefaultAsync(t => t.Id == tripAssignmentDto.TripId);
                var driver = await _unitOfWork.User.GetFirstOrDefaultAsync(d => d.Id == tripAssignmentDto.DriverId);

                if (trip == null || driver == null)
                {
                    return NotFound(new { code = 404, message = "Không tìm thấy tài xế hoặc chuyến đi" });
                }

                // Kiểm tra xem đã có đăng ký nào cho chuyến đi này chưa
                var existingAssignment = await _unitOfWork.TripsAssignments.GetFirstOrDefaultAsync(ta =>
                    ta.TripId == tripAssignmentDto.TripId &&
                    ta.DriverId == tripAssignmentDto.DriverId);

                if (existingAssignment != null)
                {
                    // Cập nhật trạng thái nếu đã có đăng ký
                    existingAssignment.Status = "Pending"; // Hoặc trạng thái khác nếu cần
                    _unitOfWork.TripsAssignments.Update(existingAssignment); // Cập nhật lại đối tượng
                }
                else
                {
                    // Nếu không có đăng ký, trả về thông báo lỗi
                    return NotFound(new { code = 404, message = "Không có đăng ký nào cho chuyến đi này." });
                }

                await _unitOfWork.SaveAsync();

                return Ok(new { code = 200, message = "Tài xế đã đăng ký chuyến đi thành công", data = existingAssignment });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = "Có lỗi xảy ra trên server", error = ex.Message });
            }
        }
    }
}

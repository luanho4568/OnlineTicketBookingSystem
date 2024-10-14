using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;

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
    }
}

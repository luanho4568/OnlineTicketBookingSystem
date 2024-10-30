using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models.DTO;
using OnlineTicketBookingSystem.Utility;

namespace OnlineTicketBookingSystem.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TripAssignmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtService _jwtService;

        public TripAssignmentController(IUnitOfWork unitOfWork, JwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
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
        [HttpGet("GetAllTripAssignmentByUserId")]
        public async Task<IActionResult> GetTripAssignmentById(string? token)
        {
            try
            {
                var claims = _jwtService.ValidateAndDecodeToken(token);
                if (!claims.TryGetValue("nameid", out string nameid))
                {
                    return NotFound(new { code = 401, message = "Không tìm thấy user" });
                }

                // Lấy user từ database bằng userId
                await _unitOfWork.Trips.GetAllAsync(includeProperties: "Buses,StartProvince,EndProvince");
                var tripAssignments = await _unitOfWork.TripsAssignments.GetAllAsync(x => x.DriverId.ToString() == nameid, includeProperties: "Trips,Driver");
                if (tripAssignments == null || !tripAssignments.Any()) return Ok(new { code = 404, message = "Lấy danh sách phê duyệt thất bại!", data = tripAssignments });
                return Ok(new { code = 200, message = "Lấy danh sách phê duyệt thành công!", data = tripAssignments });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
            }
        }
        [HttpPost("HandleTrip")]
        public async Task<IActionResult> HandleTrip([FromBody] TripAssignmentDTO tripAssignmentDto)
        {
            try
            {
                // Kiểm tra xem TripId và DriverId có hợp lệ không  
                var tripAssignment = await _unitOfWork.TripsAssignments.GetFirstOrDefaultAsync(t => t.Id == tripAssignmentDto.TripAssignmentId, includeProperties: "Trips");
                if (tripAssignment == null)
                {
                    return NotFound(new { code = 404, message = "Không tìm thấy đăng ký chuyến đi." });
                }
                if (tripAssignment.Status == "Expired")
                {
                    return NotFound(new { code = 404, message = "Đăng ký chuyến đi đã hết hạn." });
                }

                // Kiểm tra xem tài xế có được gán cho chuyến đi này chưa
                if (tripAssignment.DriverId != null && tripAssignment.DriverId != tripAssignmentDto.DriverId)
                {
                    return BadRequest(new { code = 400, message = "Chuyến đi đã có tài xế khác." });
                }
                switch (tripAssignmentDto.Action)
                {
                    case "Empty":
                        tripAssignment.Status = SD.TripAssignmentStatus_Pending;
                        break;

                    case "Accept":
                        if (tripAssignment.Status == "Pending")
                        {
                            tripAssignment.Status = SD.TripAssignmentStatus_Approved;
                        }
                        else if (tripAssignment.Status == "Approved")
                        {
                            tripAssignment.Status = SD.TripAssignmentStatus_Departing;
                            tripAssignment.Trips.Status = SD.TripStatus_Departing;
                        }
                        else if (tripAssignment.Status == "Departing")
                        {
                            if (tripAssignment.Trips.DepartureDate > DateTime.Now)
                            {
                                return NotFound(new { message = "Chưa đến thời gian khởi hành" });
                            }
                            else
                            {
                                tripAssignment.Trips.Status = SD.TripStatus_Completed;
                                tripAssignment.Status = SD.TripAssignmentStatus_Complated;
                            }
                        }
                        else
                        {
                            return BadRequest(new { code = 400, message = "Chuyến đi không thể được chấp nhận ở trạng thái hiện tại." });
                        }
                        break;

                    case "Cancel":
                        tripAssignment.Status = SD.TripAssignmentStatus_Empty; // Đặt lại trạng thái về "Empty"
                        tripAssignment.Trips.Status = SD.TripStatus_Scheduled;
                        tripAssignment.DriverId = null;
                        break;

                    default:
                        return BadRequest(new { code = 400, message = "Hành động không hợp lệ." });
                }

                if (tripAssignment.Status != "Empty")
                {
                    tripAssignment.DriverId = tripAssignmentDto.DriverId;
                }
                _unitOfWork.TripsAssignments.Update(tripAssignment);

                await _unitOfWork.SaveAsync();

                return Ok(new { code = 200, message = "Tài xế đã đăng ký chuyến đi thành công", data = tripAssignment });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = "Có lỗi xảy ra trên server", error = ex.Message });
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Utility;

namespace OnlineTicketBookingSystem.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RouteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllRoute")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var routes = await _unitOfWork.Trips.GetAllAsync(includeProperties: "Buses,StartProvince,EndProvince");
                if (routes == null || !routes.Any()) return Ok(new { code = 404, message = "Lấy danh sách tuyến xe thất bại!", data = routes });
                foreach (var route in routes)
                {
                    if (route.DepartureDate?.Date < DateTime.Now.Date && route.Status != SD.TripAssignmentStatus_Approved)
                    {
                        var tripAssignments = await _unitOfWork.TripsAssignments.GetAllAsync(x => x.TripId == route.Id);
                        if (tripAssignments != null && tripAssignments.Any())
                        {
                            foreach (var assignment in tripAssignments)
                            {
                                assignment.Status = SD.TripAssignmentStatus_Expired;
                                _unitOfWork.TripsAssignments.Update(assignment);

                                var seats = await _unitOfWork.Seats.GetAllAsync(x => x.BusId == route.BusId);
                                if (seats != null && seats.Any())
                                {
                                    foreach (var seat in seats)
                                    {
                                        seat.Status = SD.SeatStatus_Empty;
                                        _unitOfWork.Seats.Update(seat);
                                    }
                                }
                            }
                        }
                        route.Status = SD.TripAssignmentStatus_Expired;
                        _unitOfWork.Trips.Update(route);
                        await _unitOfWork.SaveAsync();
                    }
                }
                return Ok(new { code = 200, message = "Lấy danh sách tuyến xe thành công!", data = routes });
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
                var trip = await _unitOfWork.Trips.GetFirstOrDefaultAsync(u => u.Id == id);
                if (trip == null) return NotFound(new { code = 404, message = "Không tìm thấy tuyến xe" });
                var tripAssignments = await _unitOfWork.TripsAssignments.GetAllAsync(x => x.TripId == trip.Id);
                if (tripAssignments != null && tripAssignments.Any())
                {
                    await _unitOfWork.TripsAssignments.RemoveRangeAsync(tripAssignments);
                }
                await _unitOfWork.Trips.RemoveAsync(trip);
                await _unitOfWork.SaveAsync();
                return Ok(new { code = 200, message = "Xoá thành công" });
            }
            catch (Exception e)
            {

                return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
            }
        }
    }
}

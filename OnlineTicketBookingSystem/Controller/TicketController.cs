using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;
using OnlineTicketBookingSystem.Models.ViewModel;
using OnlineTicketBookingSystem.Utility;

namespace OnlineTicketBookingSystem.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public TicketController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetAllTickets")]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? date = null)
        {
            try
            {
                await _unitOfWork.Trips.GetAllAsync(includeProperties: "Buses,StartProvince,EndProvince");
                var tickets = await _unitOfWork.Tickets.GetAllAsync(includeProperties: "Trips,User,Seats");

                if (date.HasValue)
                {
                    tickets = tickets.Where(t => t.CreatedAt?.Date == date.Value.Date).ToList();
                }

                if (tickets == null || !tickets.Any()) return Ok(new { code = 404, message = "Lấy danh sách vé thất bại!", data = tickets });
                return Ok(new { code = 200, message = "Lấy danh sách vé thành công!", data = tickets });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
            }
        }

        [HttpPost("BookTickets")]
        public async Task<IActionResult> BookTickets([FromBody] TicketVM ticketVM)
        {
            if (ticketVM.SeatIds == null || !ticketVM.SeatIds.Any())
            {
                return BadRequest(new { code = 404, message = "Vui lòng chọn ít nhất một ghế." });
            }

            foreach (var seatId in ticketVM.SeatIds)
            {
                var ticket = new Tickets
                {
                    UserId = ticketVM.UserId,
                    TripId = ticketVM.TripId,
                    SeatId = seatId,
                    Price = ticketVM.Price,
                    Status = SD.TicketStatus_Pending,
                };

                await _unitOfWork.Tickets.AddAsync(ticket);
            }

            await _unitOfWork.SaveAsync();

            return Ok(new { code = 200, message = "Đặt vé thành công!" });
        }

    }
}

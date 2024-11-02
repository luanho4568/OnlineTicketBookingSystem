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
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Id == ticketVM.UserId);
            if (user == null)
            {
                return BadRequest(new { code = 404, message = "Không tìm thấy user" });
            }
            decimal totalCost = ticketVM.Price * ticketVM.SeatIds.Count;
            if (user.Balance < totalCost)
            {
                return BadRequest(new { code = -1, message = "Số dư không đủ" });
            }
            var trip = await _unitOfWork.Trips.GetFirstOrDefaultAsync(t => t.Id == ticketVM.TripId);
            if (trip == null || trip.BusId == null)
            {
                return BadRequest(new { code = 404, message = "Không tìm thấy chuyến xe hoặc xe buýt." });
            }

            var bus = await _unitOfWork.Buses.GetFirstOrDefaultAsync(b => b.Id == trip.BusId);
            if (bus == null)
            {
                return BadRequest(new { code = 404, message = "Không tìm thấy xe buýt." });
            }
            foreach (var seatId in ticketVM.SeatIds)
            {
                var seat = await _unitOfWork.Seats.GetFirstOrDefaultAsync(s => s.Id == seatId);
                if (seat == null || seat.Status != SD.SeatStatus_Empty)
                {
                    return BadRequest(new { code = 400, message = $"Ghế với ID {seatId} không thể đặt." });
                }
                seat.Status = SD.SeatStatus_Sold;
                _unitOfWork.Seats.Update(seat);
                var ticket = new Tickets
                {
                    UserId = ticketVM.UserId,
                    TripId = ticketVM.TripId,
                    SeatId = seatId,
                    Price = ticketVM.Price,
                    Status = SD.TicketStatus_Completed,
                };

                await _unitOfWork.Tickets.AddAsync(ticket);
            }
            user.Balance -= totalCost;
            _unitOfWork.User.Update(user);
            bus.EmptySeats -= ticketVM.SeatIds.Count;
            if (bus.EmptySeats < 0) bus.EmptySeats = 0;
            await _unitOfWork.SaveAsync();

            return Ok(new { code = 200, message = "Đặt vé thành công!" });
        }

    }
}

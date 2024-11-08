using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;
using OnlineTicketBookingSystem.Models.ViewModel;
using OnlineTicketBookingSystem.Utility;

namespace AdminDriverDashboard.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class RouteController : Controller
    {
        private readonly JwtService _jwt;
        private readonly IUnitOfWork _unitOfWork;
        public RouteController(JwtService jwt, IUnitOfWork unitOfWork)
        {
            _jwt = jwt;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(string startPoint, string endPoint, DateTime departureDate)
        {
            var provinceGetAll = await _unitOfWork.Province.GetAllAsync();

            var seats = await _unitOfWork.Seats.GetAllAsync();

            IEnumerable<Trips> trips = await _unitOfWork.Trips.GetAllAsync(t => t.StartPoint == startPoint &&
                                                                             t.EndPoint == endPoint &&
                                                                             t.DepartureDate == departureDate.Date &&
                                                                             t.Status == "Scheduled",
                                                                             includeProperties: "Buses");

            var filteredTrips = new List<Trips>();

            foreach (var trip in trips)
            {
                var bus = trip.Buses;

                if (bus != null)
                {
                    var seatG1 = seats.FirstOrDefault(seat => seat.BusId == bus.Id && seat.SeatNumber == "G1");

                    if (seatG1 != null && seatG1.Status == "Driver")
                    {
                        filteredTrips.Add(trip);
                    }
                }
            }

            // Trả dữ liệu về View với các chuyến đi đã lọc
            TripVM tripVM = new TripVM
            {
                Province = provinceGetAll.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Code
                }),
                TripList = filteredTrips
            };

            return View(tripVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(TripVM tripVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(tripVM);
                }
                var trips = await _unitOfWork.Trips.GetAllAsync(
                        t => t.StartPoint == tripVM.Trip.StartPoint &&
                             t.EndPoint == tripVM.Trip.EndPoint &&
                             t.DepartureDate == tripVM.Trip.DepartureDate &&
                     t.Status == "Scheduled");
                if (!trips.Any())
                {
                    return RedirectToAction("Index", "NoTrips");
                }
                return RedirectToAction(nameof(Index), new
                {
                    startPoint = tripVM.Trip.StartPoint,
                    endPoint = tripVM.Trip.EndPoint,
                    departureDate = tripVM.Trip.DepartureDate.Value.ToString("yyyy-MM-dd") // Định dạng lại nếu cần
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("Đã xảy ra lỗi: " + e.Message);
                return View(tripVM);
            }
        }
    }
}

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
            IEnumerable<Trips> trips = new List<Trips>();
            trips = await _unitOfWork.Trips.GetAllAsync(t => t.StartPoint == startPoint &&
            t.EndPoint == endPoint &&
            t.DepartureDate == departureDate.Date,
            includeProperties: "Buses");
            TripVM tripVM = new TripVM
            {
                Province = provinceGetAll.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Code
                }),
                TripList = trips
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

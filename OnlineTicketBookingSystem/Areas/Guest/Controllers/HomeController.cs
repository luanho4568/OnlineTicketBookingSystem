using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models.ViewModel;
using OnlineTicketBookingSystem.Utility;

namespace AdminDriverDashboard.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class HomeController : Controller
    {
        private readonly JwtService _jwt;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(JwtService jwt, IUnitOfWork unitOfWork)
        {
            _jwt = jwt;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var provinceGetAll = await _unitOfWork.Province.GetAllAsync();
            TripVM tripVM = new TripVM
            {
                Province = provinceGetAll.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Code
                })
            };
            return View(tripVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(TripVM tripVM)
        {
            try
            {
                var trips = await _unitOfWork.Trips.GetAllAsync(
                        t => t.StartPoint == tripVM.Trip.StartPoint &&
                             t.EndPoint == tripVM.Trip.EndPoint &&
                             t.DepartureDate == (tripVM.Trip.DepartureDate ?? null) &&
                             t.Status == "Scheduled");
                var checkTrips = string.IsNullOrEmpty(tripVM.Trip.StartPoint) || string.IsNullOrEmpty(tripVM.Trip.EndPoint)
                    || string.IsNullOrEmpty(tripVM.Trip.DepartureDate.ToString());
                if (checkTrips)
                {
                    TempData["Error"] = "Vui lòng nhập đầy đủ thông tin chuyến đi";
                    return View(tripVM);
                }
                if (!trips.Any())
                {
                    return RedirectToAction("Index", "NoTrips");
                }
                return RedirectToAction("Index", "Route", new
                {
                    startPoint = tripVM.Trip.StartPoint,
                    endPoint = tripVM.Trip.EndPoint,
                    departureDate = tripVM.Trip.DepartureDate.Value.ToString("yyyy-MM-dd") ?? null
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

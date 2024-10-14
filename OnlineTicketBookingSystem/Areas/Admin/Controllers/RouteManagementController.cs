using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;
using OnlineTicketBookingSystem.Models.ViewModel;
using OnlineTicketBookingSystem.Utility;

namespace AdminDriverDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RouteManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IUnitOfWork _unitOfWork;
        public RouteManagementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> CreateRoute()
        {
            var provinceGetAll = await _unitOfWork.Province.GetAllAsync();
            var busesGetAll = await _unitOfWork.Buses.GetAllAsync();
            TripVM tripVM = new TripVM();
            tripVM.Trip = new Trips();
            tripVM.Province = provinceGetAll.Select(
                 x => new SelectListItem()
                 {
                     Text = x.Name,
                     Value = x.Code,
                 });
            tripVM.Buses = busesGetAll.Select(
                 x => new SelectListItem()
                 {
                     Text = x.Name,
                     Value = x.Id.ToString(),
                 });
            return View(tripVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoute(TripVM tripVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine(error.ErrorMessage);
                        }
                    }
                    return View(tripVM);
                }
                var isDuplicateTrip = await _unitOfWork.Trips.GetFirstOrDefaultAsync(
                    x => x.DepartureDate == tripVM.Trip.DepartureDate
                      && x.DepartureTime == tripVM.Trip.DepartureTime
                      && x.EstimatedArrivalTime == tripVM.Trip.EstimatedArrivalTime
                      && x.BusId == tripVM.Trip.BusId
                );
                if (isDuplicateTrip != null)
                {
                    // Nếu trùng, trả về thông báo lỗi
                    TempData["Eror"] = "Tuyến xe đã tồn tại với cùng ngày, giờ khởi hành và xe";
                    return View(tripVM);
                }
                var trip = tripVM.Trip;
                trip.Id = Guid.NewGuid();
                trip.Status = SD.TripStatus_Scheduled;
                trip.UpdatedAt = DateTime.Now.ToString();
                await _unitOfWork.Trips.AddAsync(trip);
                TripAssignments tripAssignments = new TripAssignments
                {
                    DriverId = null,
                    TripId = trip.Id,
                    Status = SD.TripAssignmentStatus_Empty
                };
                await _unitOfWork.TripsAssignments.AddAsync(tripAssignments);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng ký: " + ex.Message);
                return View(tripVM);
            }
        }

        public async Task<IActionResult> EditRoute(Guid id)
        {
            var trip = await _unitOfWork.Trips.GetFirstOrDefaultAsync(u => u.Id == id);
            if (trip == null)
            {
                return NotFound();
            }

            var provinceGetAll = await _unitOfWork.Province.GetAllAsync();
            var busesGetAll = await _unitOfWork.Buses.GetAllAsync();

            TripVM tripVM = new TripVM
            {
                Trip = trip,
                Province = provinceGetAll.Select(
                    x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code,
                    }),
                Buses = busesGetAll.Select(
                    x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    })
            };

            return View(tripVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoute(TripVM tripVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            TempData["Eror"] = "Chỉnh sửa người dùng thất bại";
                            Console.WriteLine(error.ErrorMessage);
                        }
                    }
                    return View(tripVM);
                }

                var trip = await _unitOfWork.Trips.GetFirstOrDefaultAsync(u => u.Id == tripVM.Trip.Id);
                if (trip == null)
                {
                    return View(tripVM);
                }
                var isDuplicateTrip = await _unitOfWork.Trips.GetFirstOrDefaultAsync(
                    x => x.DepartureDate == tripVM.Trip.DepartureDate
                      && x.DepartureTime == tripVM.Trip.DepartureTime
                      && x.EstimatedArrivalTime == tripVM.Trip.EstimatedArrivalTime
                      && x.BusId == tripVM.Trip.BusId
                      && x.Id != tripVM.Trip.Id // Đảm bảo không trùng với chính tuyến đang được chỉnh sửa
                );
                if (isDuplicateTrip != null)
                {
                    // Nếu trùng, trả về thông báo lỗi
                    TempData["Eror"] = "Tuyến xe đã tồn tại với cùng ngày, giờ khởi hành và xe";
                    return View(tripVM);
                }
                trip.StartPoint = tripVM.Trip.StartPoint;
                trip.EndPoint = tripVM.Trip.EndPoint;
                trip.BusId = tripVM.Trip.BusId;
                trip.DepartureDate = tripVM.Trip.DepartureDate;
                trip.Distance = tripVM.Trip.Distance;
                trip.DepartureTime = tripVM.Trip.DepartureTime;
                trip.Price = tripVM.Trip.Price;
                trip.EstimatedArrivalTime = tripVM.Trip.EstimatedArrivalTime;
                trip.UpdatedAt = DateTime.Now.ToString();
                var tripAssignments = await _unitOfWork.TripsAssignments.GetFirstOrDefaultAsync(x => x.TripId == tripVM.Trip.Id);
                bool isApprovedAssignment = tripAssignments.Status == SD.TripAssignmentStatus_Approved && tripAssignments.DriverId.HasValue;
                if (tripVM.Trip.Status == SD.TripStatus_Cancelled)
                {
                    trip.Status = tripVM.Trip.Status;
                    tripAssignments.Status = SD.TripAssignmentStatus_Empty;
                    _unitOfWork.TripsAssignments.Update(tripAssignments);
                }
                else if (tripVM.Trip.Status == SD.TripStatus_Scheduled)
                {
                    trip.Status = tripVM.Trip.Status;
                    tripAssignments.Status = SD.TripAssignmentStatus_Empty;
                    _unitOfWork.TripsAssignments.Update(tripAssignments);
                }
                else if (isApprovedAssignment)
                {
                    trip.Status = tripVM.Trip.Status;
                }
                else
                {
                    TempData["Error"] = "Không thể cập nhật trạng thái chuyến đi vì không có phân công nào được phê duyệt.";
                }
                _unitOfWork.Trips.Update(trip);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật: " + ex.Message);
                return View(tripVM);
            }
        }
    }
}

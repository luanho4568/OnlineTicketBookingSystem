using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;
using OnlineTicketBookingSystem.Utility;

namespace AdminDriverDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;

        public BusController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult CreateBus()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBus(Buses bus, IFormFile file)
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
                    return View(bus);
                }
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null && file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\buses");
                    var extension = Path.GetExtension(file.FileName);
                    if (bus.Image != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, bus.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    bus.Image = @"images\buses\" + fileName + extension;
                }
                bus.Id = Guid.NewGuid();
                bus.EmptySeats = bus.TotalSeats - 1;
                bus.Status = true;
                await _unitOfWork.Buses.AddAsync(bus);
                for (int i = 1; i <= bus.TotalSeats; i++)
                {
                    Seats seat = new Seats
                    {
                        BusId = bus.Id,
                        SeatNumber = "G" + i,
                        Status = SD.SeatStatus_Empty
                    };
                    await _unitOfWork.Seats.AddAsync(seat);
                }
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi tạo mới xe: " + ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> EditBus(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bus = await _unitOfWork.Buses.GetFirstOrDefaultAsync(item => item.Id == id);
            if (bus == null)
            {
                return NotFound();
            }
            return View(bus);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBus(Buses bus, IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var existingBus = await _unitOfWork.Buses.GetFirstOrDefaultAsync(item => item.Id == bus.Id);

            // Nếu không tìm thấy bus cũ, trả về NotFound
            if (existingBus == null)
            {
                return NotFound();
            }

            // Cập nhật thuộc tính của existingBus
            if (file == null || file.Length == 0)
            {
                // Giữ nguyên giá trị cũ của existingBus.Image
                existingBus.Image = existingBus.Image;
            }
            else
            {
                // Nếu có file mới được tải lên
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\buses");
                var extension = Path.GetExtension(file.FileName);

                // Xóa hình ảnh cũ nếu có
                if (!string.IsNullOrEmpty(existingBus.Image))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, existingBus.Image.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Tải file mới lên
                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    await file.CopyToAsync(fileStreams);
                }
                existingBus.Image = @"images\buses\" + fileName + extension; // Cập nhật đường dẫn hình ảnh
            }

            // Cập nhật thời gian
            existingBus.Name = bus.Name;
            existingBus.LicensePlate = bus.LicensePlate;
            existingBus.BusType = bus.BusType;
            existingBus.Status = bus.Status;
            existingBus.UpdatedAt = DateTime.Now.ToString();
            if (existingBus.TotalSeats < bus.TotalSeats)
            {
                // Thêm ghế mới khi số ghế tăng lên
                for (int i = (int)(existingBus.TotalSeats + 1); i <= bus.TotalSeats; i++)
                {
                    var seatNumber = "G" + i;
                    Seats seat = new Seats
                    {
                        BusId = bus.Id,
                        SeatNumber = seatNumber,
                        Status = seatNumber
                    };
                    await _unitOfWork.Seats.AddAsync(seat);
                }
            }
            else if (existingBus.TotalSeats > bus.TotalSeats)
            {
                // Xóa các ghế thừa khi số ghế giảm xuống
                var seatsToRemove = (await _unitOfWork.Seats.GetAllAsync(s => s.BusId == bus.Id))
                                                .Where(s => int.Parse(s.SeatNumber.Substring(1)) > bus.TotalSeats)
                                                .ToList();
                await _unitOfWork.Seats.RemoveRangeAsync(seatsToRemove);
            }

            // Cập nhật số ghế
            existingBus.TotalSeats = bus.TotalSeats;
            existingBus.EmptySeats = bus.TotalSeats - 1;
            _unitOfWork.Buses.Update(existingBus);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Seats(Guid busId)
        {
            ViewBag.BusId = busId;
            return View();
        }


        public async Task<IActionResult> EditSeat(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var seat = await _unitOfWork.Seats.GetFirstOrDefaultAsync(item => item.Id == id, includeProperties: "Buses");
            ViewBag.busId = seat.BusId;
            if (seat == null)
            {
                return NotFound();
            }
            return View(seat);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSeat(Seats seat)
        {
            var existingSeat = await _unitOfWork.Seats.GetFirstOrDefaultAsync(item => item.Id == seat.Id);
            existingSeat.Status = seat.Status;
            _unitOfWork.Seats.Update(existingSeat);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Seats), new { busId = existingSeat.BusId });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;

namespace OnlineTicketBookingSystem.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BusController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;


        public BusController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {

            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("GetSeatsByBusId/{busId}")]
        public async Task<IActionResult> GetSeatsByBusId(Guid? busId)
        {
            try
            {
                var seats = await _unitOfWork.Seats.GetAllAsync(s => s.BusId == busId, includeProperties: "Buses");

                if (seats == null || !seats.Any())
                    return Ok(new { code = 404, message = "Không tìm thấy ghế cho xe bus này!", data = seats });

                return Ok(new { code = 200, message = "Lấy danh sách ghế thành công!", data = seats });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
            }
        }

        [HttpGet("GetAllBus")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var buses = await _unitOfWork.Buses.GetAllAsync();
                if (buses == null || !buses.Any()) return Ok(new { code = 404, message = "Lấy danh sách xe thất bại!", data = buses });
                return Ok(new { code = 200, message = "Lấy danh sách xe thành công!", data = buses });
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
                var buses = await _unitOfWork.Buses.GetFirstOrDefaultAsync(u => u.Id == id);
                if (buses == null) return NotFound(new { code = 404, message = "Không tìm thấy xe bus!" });
                var seatsToRemove = await _unitOfWork.Seats.GetAllAsync(s => s.BusId == buses.Id);
                if (seatsToRemove != null && seatsToRemove.Any())
                {
                    await _unitOfWork.Seats.RemoveRangeAsync(seatsToRemove);
                }
                var _busesImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "images", "buses");
                if (!string.IsNullOrEmpty(buses.Image))
                {
                    var imagePath = Path.Combine(_busesImagePath, Path.GetFileName(buses.Image));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath); // Xóa file hình ảnh
                    }
                }
                await _unitOfWork.Buses.RemoveAsync(buses);
                await _unitOfWork.SaveAsync();
                return Ok(new { code = 200, message = "Xoá thành công!" });
            }
            catch (Exception e)
            {

                return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
            }
        }
    }
}

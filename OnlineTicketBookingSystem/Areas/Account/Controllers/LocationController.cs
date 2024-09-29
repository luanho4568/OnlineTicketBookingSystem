using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;

namespace AdminDriverDashboard.Areas.Account.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public LocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetDistrictsByProvince")]
        public async Task<IActionResult> GetDistrictsByProvince(string provinceCode)
        {
            try
            {
                if (string.IsNullOrEmpty(provinceCode))
                {
                    return BadRequest(new { message = "Mã tỉnh không hợp lệ." });
                }

                var districts = await _unitOfWork.District.GetAllAsync(d => d.ProvinceCode == provinceCode);
                if (districts == null || !districts.Any())
                {
                    return NotFound(new { message = "Không tìm thấy quận/huyện cho tỉnh đã chọn." });
                }

                return Ok(new SelectList(districts, "Code", "Name"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi ở server.", error = ex.Message });
            }
        }

        [HttpGet("GetWardsByDistrict")]
        public async Task<IActionResult> GetWardsByDistrict(string districtCode)
        {
            try
            {
                if (string.IsNullOrEmpty(districtCode))
                {
                    return BadRequest(new { message = "Mã quận/huyện không hợp lệ." });
                }

                var wards = await _unitOfWork.Ward.GetAllAsync(w => w.DistrictCode == districtCode);
                if (wards == null || !wards.Any())
                {
                    return NotFound(new { message = "Không tìm thấy phường/xã cho quận/huyện đã chọn." });
                }

                return Ok(new SelectList(wards, "Code", "Name"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi ở server.", error = ex.Message });
            }
        }
    }
}

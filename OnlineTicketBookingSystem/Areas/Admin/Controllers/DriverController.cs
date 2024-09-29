using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Utility;

namespace AdminDriverDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DriverController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserServices _userServices;
        private IWebHostEnvironment _webHostEnvironment;

        public DriverController(IUnitOfWork unitOfWork, UserServices userServices, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userServices = userServices;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        ////API
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        return await _userService.GetUsersByRoleAsync("Driver");

        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
        //    }
        //}

        //[HttpDelete]
        //public async Task<IActionResult> Delete(string? id)
        //{
        //    try
        //    {
        //        return await _userService.DeleteUser(id);

        //    }
        //    catch (Exception e)
        //    {

        //        return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
        //    }
        //}
        //[HttpPost]
        //public async Task<IActionResult> CreateUser([FromBody] ApplicationUser user)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        //            return BadRequest(new { status = false, message = "Dữ liệu lỗi", errors });
        //        }
        //        return await _userService.CreateUserService(user, "Driver");
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, new { message = "Có lỗi xảy ra ở server", error = e.Message });
        //    }
        //}

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;
using OnlineTicketBookingSystem.Models.ViewModel;

namespace AdminDriverDashboard.Areas.Account.Controllers
{
    [Area("Account")]
    public class RegisterController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
        public RegisterController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var groupGetAll = await _unitOfWork.Group.GetAllAsync();
            var provinceGetAll = await _unitOfWork.Province.GetAllAsync();
            UserVM userVM = new UserVM();
            userVM.User = new User();
            userVM.Group = groupGetAll.Select(
                 x => new SelectListItem()
                 {
                     Text = x.NameVI,
                     Value = x.Id.ToString(),
                 });
            userVM.Province = provinceGetAll.Select(
                 x => new SelectListItem()
                 {
                     Text = x.Name,
                     Value = x.Code,
                 });
            return View(userVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserVM userVM)
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
                    return View(userVM);
                }
                var user = userVM.User;
                var confirmPassword = Request.Form["ConfirmPassword"];
                if (user.Password != confirmPassword)
                {
                    ModelState.AddModelError("Password", "Mật khẩu và xác nhận mật khẩu không khớp.");
                    return View(userVM);
                }
                user.Id = Guid.NewGuid();
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = hashPassword;
                user.CreatedAt = DateTime.Now.ToString();
                user.UpdatedAt = DateTime.Now.ToString();
                await _unitOfWork.User.AddAsync(user);
                await _unitOfWork.SaveAsync();
                return RedirectToAction("Index", "Auth");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng ký: " + ex.Message);
                return View(userVM);
            }
        }


    }
}

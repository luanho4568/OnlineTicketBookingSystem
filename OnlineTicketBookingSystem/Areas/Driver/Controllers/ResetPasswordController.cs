using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models.ViewModel;
using OnlineTicketBookingSystem.Utility;

namespace AdminDriverDashboard.Areas.Driver.Controllers
{
    [Area("Driver")]
    public class ResetPasswordController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtService _jwtService;

        public ResetPasswordController(IUnitOfWork unitOfWork, JwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string? authToken, ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Giải mã token và lấy các claims
            var claims = _jwtService.ValidateAndDecodeToken(authToken);
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim Type: {claim.Key}, Claim Value: {claim.Value}");
            }
            if (!claims.TryGetValue("nameid", out string nameid))
            {
                TempData["Error"] = "Token không hợp lệ hoặc không có userId.";
                return View(model);
            }

            // Lấy user từ database bằng userId
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Id.ToString() == nameid); // Đảm bảo so sánh kiểu đúng

            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy người dùng.";
                return View(model);
            }

            // Kiểm tra mật khẩu cũ
            if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
            {
                TempData["Error"] = "Mật khẩu cũ không chính xác.";
                return View(model);
            }

            // Hash mật khẩu mới
            var newHashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

            // Cập nhật mật khẩu mới vào cơ sở dữ liệu
            user.Password = newHashedPassword;
            user.UpdatedAt = DateTime.Now.ToString(); // Cập nhật thời gian sửa đổi

            _unitOfWork.User.Update(user); // Cập nhật user
            await _unitOfWork.SaveAsync(); // Lưu thay đổi vào database

            TempData["Success"] = "Thay đổi mật khẩu thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}

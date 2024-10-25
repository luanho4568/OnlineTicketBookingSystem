using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models.ViewModel;
using OnlineTicketBookingSystem.Utility;

namespace AdminDriverDashboard.Areas.Driver.Controllers
{
    [Area("Driver")]
    public class ProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _env;
        private readonly JwtService _jwtService;

        public ProfileController(IUnitOfWork unitOfWork, IWebHostEnvironment env, JwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                Console.WriteLine("Người dùng không tồn tại");
                return View();
            }

            var groupGetAll = await _unitOfWork.Group.GetAllAsync();
            var provinceGetAll = await _unitOfWork.Province.GetAllAsync();
            var districtGetAll = await _unitOfWork.District.GetAllAsync(d => d.ProvinceCode == user.ProvinceCode);
            var wardGetAll = await _unitOfWork.Ward.GetAllAsync(w => w.DistrictCode == user.DistrictCode);

            UserVM userVM = new UserVM
            {
                User = user,
                Group = groupGetAll.Select(x => new SelectListItem
                {
                    Text = x.NameVI,
                    Value = x.Id.ToString(),
                }),
                Province = provinceGetAll.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Code,
                }),
                District = districtGetAll.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Code,
                }),
                Ward = wardGetAll.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Code,
                })
            };
            return View(userVM); // Sửa lại trả về userVM
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserVM userVM, IFormFile file)
        {
            string wwwRootPath = _env.WebRootPath;
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Id == userVM.User.Id);
            if (user == null)
            {
                Console.WriteLine("Người dùng không tồn tại");
                return View(userVM);
            }
            if (file == null || file.Length == 0)
            {
                user.Avatar = user.Avatar;
            }            // Kiểm tra và xử lý file ảnh
            else
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\users");
                var extension = Path.GetExtension(file.FileName);

                // Xóa hình ảnh cũ nếu có
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, user.Avatar.TrimStart('\\'));
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
                user.Avatar = @"images\users\" + fileName + extension; // Cập nhật đường dẫn hình ảnh
            }

            // Cập nhật thông tin người dùng
            user.Id = userVM.User.Id;
            user.FullName = userVM.User.FullName;
            user.Gender = userVM.User.Gender;
            user.PhoneNumber = userVM.User.PhoneNumber;
            user.ProvinceCode = userVM.User.ProvinceCode;
            user.DistrictCode = userVM.User.DistrictCode;
            user.WardCode = userVM.User.WardCode;
            user.Address = userVM.User.Address;
            user.DateOfBirth = userVM.User.DateOfBirth;
            user.UpdatedAt = DateTime.Now.ToString();

            _unitOfWork.User.Update(user);
            await _unitOfWork.SaveAsync();

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction(nameof(Index), new { id = user.Id });
        }

    }
}

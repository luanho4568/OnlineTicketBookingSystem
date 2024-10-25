using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;
using OnlineTicketBookingSystem.Models.ViewModel;

namespace AdminDriverDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;

        public UserController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UserIndex()
        {
            return View();
        }
        public async Task<IActionResult> Profile(Guid id)
        {
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                Console.WriteLine("Người dùng không tồn tại");
                return View();
            }

            var provinceGetAll = await _unitOfWork.Province.GetAllAsync();
            var districtGetAll = await _unitOfWork.District.GetAllAsync(d => d.ProvinceCode == user.ProvinceCode);
            var wardGetAll = await _unitOfWork.Ward.GetAllAsync(w => w.DistrictCode == user.DistrictCode);

            UserVM userVM = new UserVM
            {
                User = user,

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
        public async Task<IActionResult> Profile(UserVM userVM, IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
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
            return RedirectToAction(nameof(Profile), new { id = user.Id });
        }
        public async Task<IActionResult> CreateUser()
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
        public async Task<IActionResult> CreateUser(UserVM userVM)
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
                user.UpdatedAt = DateTime.Now.ToString();
                await _unitOfWork.User.AddAsync(user);
                await _unitOfWork.SaveAsync();
                if (user.GroupId == 2)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (user.GroupId == 3)
                {
                    return RedirectToAction(nameof(UserIndex));
                }
                return View(userVM);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng ký: " + ex.Message);
                return View(userVM);
            }
        }
        public async Task<IActionResult> EditUser(Guid id)
        {
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var groupGetAll = await _unitOfWork.Group.GetAllAsync();
            var provinceGetAll = await _unitOfWork.Province.GetAllAsync();
            var districtGetAll = await _unitOfWork.District.GetAllAsync(d => d.ProvinceCode == user.ProvinceCode);
            var wardGetAll = await _unitOfWork.Ward.GetAllAsync(w => w.DistrictCode == user.DistrictCode);

            UserVM userVM = new UserVM
            {
                User = user,
                Group = groupGetAll.Select(
                    x => new SelectListItem
                    {
                        Text = x.NameVI,
                        Value = x.Id.ToString(),
                    }),
                Province = provinceGetAll.Select(
                    x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code,
                    }),
                District = districtGetAll.Select(
                    x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code,
                    }),
                Ward = wardGetAll.Select(
                    x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code,
                    })
            };

            return View(userVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserVM userVM)
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
                    return View(userVM);
                }

                var user = await _unitOfWork.User.GetFirstOrDefaultAsync(u => u.Id == userVM.User.Id);
                if (user == null)
                {
                    return View(userVM);
                }
                user.FullName = userVM.User.FullName;
                user.IsActive = userVM.User.IsActive;
                user.GroupId = userVM.User.GroupId;
                user.Address = userVM.User.Address;
                user.ProvinceCode = userVM.User.ProvinceCode;
                user.DistrictCode = userVM.User.DistrictCode;
                user.WardCode = userVM.User.WardCode;
                user.Gender = userVM.User.Gender;
                user.DateOfBirth = userVM.User.DateOfBirth;
                user.UpdatedAt = DateTime.Now.ToString();
                _unitOfWork.User.Update(user);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật: " + ex.Message);
                return View(userVM);
            }
        }
    }
}

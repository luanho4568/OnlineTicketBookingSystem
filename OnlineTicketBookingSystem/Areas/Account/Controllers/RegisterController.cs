﻿using Microsoft.AspNetCore.Mvc;
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
        public RegisterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var groupGetAll = await _unitOfWork.Group.GetAllAsync(u => u.Id != 1);
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
                var groupGetAll = await _unitOfWork.Group.GetAllAsync(u => u.Id != 1);
                var provinceGetAll = await _unitOfWork.Province.GetAllAsync();

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
                var user = userVM.User;
                var existingUser = await _unitOfWork.User.GetFirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng.");

                    return View(userVM);
                }
                if (string.IsNullOrEmpty(user.Password))
                {
                    ModelState.AddModelError("Password", "Mật khẩu không được để trống.");
                    return View(userVM);
                }
                var confirmPassword = Request.Form["ConfirmPassword"];
                if (user.Password != confirmPassword)
                {
                    ModelState.AddModelError("Password", "Mật khẩu và xác nhận mật khẩu không khớp.");

                    return View(userVM);
                }
                if (user.DateOfBirth > DateTime.Now)
                {
                    ModelState.AddModelError("DateOfBirth", "Vui lòng nhập đúng ngày sinh");

                    return View(userVM);
                }
                user.Id = Guid.NewGuid();
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = hashPassword;
                user.CreatedAt = DateTime.Now.ToString();
                user.UpdatedAt = DateTime.Now.ToString();
                await _unitOfWork.User.AddAsync(user);
                await _unitOfWork.SaveAsync();
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng ký: " + ex.Message);
                return View(userVM);
            }
        }
    }
}

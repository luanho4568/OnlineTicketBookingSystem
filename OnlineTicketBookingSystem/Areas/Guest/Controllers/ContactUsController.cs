using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;
using OnlineTicketBookingSystem.Utility;

namespace AdminDriverDashboard.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class ContactUsController : Controller
    {
        private readonly EmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public ContactUsController(EmailService emailService, IUnitOfWork unitOfWork)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactUsRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    request.Id = Guid.NewGuid();
                    await _emailService.SendContactUsEmailAsync(request.FullName, request.Email, request.PhoneNumber, request.Subject, request.Message);
                    await _unitOfWork.ContactUsRequest.AddAsync(request);
                    await _unitOfWork.SaveAsync();
                    TempData["Success"] = "Cảm ơn bạn đã liên hệ với chúng tôi. Chúng tôi sẽ phản hồi bạn trong thời gian sớm nhất.";
                    return View(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Ghi log hoặc xử lý lỗi nếu có
                    TempData["Error"] = "Có lỗi xảy ra khi gửi thông tin. Vui lòng thử lại sau.";
                    throw ex;
                }
            }
            else
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        TempData["Error"] = error.ErrorMessage;
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }

            return View(request);
        }
    }
}

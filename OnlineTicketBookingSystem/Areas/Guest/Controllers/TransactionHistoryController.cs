using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;
using OnlineTicketBookingSystem.Utility;

namespace AdminDriverDashboard.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class TransactionHistoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtService _jwtService;
        private readonly StripeSettings _stripeSettings;

        public TransactionHistoryController(IUnitOfWork unitOfWork, JwtService jwtService, IOptions<StripeSettings> stripeSettings)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _stripeSettings = stripeSettings.Value;
        }
        public async Task<IActionResult> Index(string token)
        {
            var claims = _jwtService.ValidateAndDecodeToken(token);
            if (!claims.TryGetValue("nameid", out string nameid))
            {
                Console.WriteLine("không có user");
            }
            var transactionHistoryList = await _unitOfWork.TransactionHistory.GetAllAsync(x => x.UserId.ToString() == nameid);
            return View(transactionHistoryList);
        }
    }
}

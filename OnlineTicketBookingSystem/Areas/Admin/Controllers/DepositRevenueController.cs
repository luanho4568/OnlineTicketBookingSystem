using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;

namespace AdminDriverDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepositRevenueController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepositRevenueController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string timeType = "day", string chartType = "bar")
        {
            startDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            endDate ??= DateTime.Now;

            var transactions = await _unitOfWork.TransactionHistory.GetAllAsync(
                th => th.CreatedAt >= startDate && th.CreatedAt <= endDate && th.Status == "paid");

            var revenueData = transactions
                .GroupBy(th => timeType.ToLower() switch
                {
                    "day" => th.CreatedAt.ToString("dd/MM/yyyy"),
                    "month" => th.CreatedAt.ToString("MM/yyyy"),
                    "year" => th.CreatedAt.ToString("yyyy"),
                    _ => th.CreatedAt.ToString("dd/MM/yyyy")
                })
                .ToDictionary(g => g.Key, g => g.Sum(th => th.Amount));

            ViewBag.TimeType = timeType;
            ViewBag.ChartType = chartType; // Lưu loại biểu đồ

            return View(revenueData);
        }

    }
}

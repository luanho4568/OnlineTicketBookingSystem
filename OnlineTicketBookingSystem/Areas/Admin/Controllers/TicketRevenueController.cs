using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;

namespace AdminDriverDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TicketRevenueController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketRevenueController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string timeType = "day", string chartType = "bar")
        {
            startDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            endDate ??= DateTime.Now;

            var tickets = await _unitOfWork.Tickets.GetAllAsync(
                th => th.CreatedAt >= startDate && th.CreatedAt <= endDate && th.Status == "Completed");

            var revenueData = tickets
                .GroupBy(th => timeType.ToLower() switch
                {
                    "day" => th.CreatedAt.HasValue ? th.CreatedAt.Value.ToString("dd/MM/yyyy") : "No Date",
                    "month" => th.CreatedAt.HasValue ? th.CreatedAt.Value.ToString("MM/yyyy") : "No Date",
                    "year" => th.CreatedAt.HasValue ? th.CreatedAt.Value.ToString("yyyy") : "No Date",
                    _ => th.CreatedAt.HasValue ? th.CreatedAt.Value.ToString("dd/MM/yyyy") : "No Date"
                })
                .ToDictionary(g => g.Key, g => g.Sum(th => th.Price ?? 0));

            ViewBag.TimeType = timeType;
            ViewBag.ChartType = chartType;

            return View(revenueData);
        }
    }
}

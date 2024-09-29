using Microsoft.AspNetCore.Mvc;

namespace AdminDriverDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TicketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

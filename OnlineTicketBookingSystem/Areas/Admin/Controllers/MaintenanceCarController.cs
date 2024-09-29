using Microsoft.AspNetCore.Mvc;

namespace AdminDriverDashboard.Areas.Admin.Controllers
{
    public class MaintenanceCarController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

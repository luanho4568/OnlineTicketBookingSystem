using Microsoft.AspNetCore.Mvc;

namespace AdminDriverDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RouteManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace AdminDriverDashboard.Areas.Driver.Controllers
{
    [Area("Driver")]
    public class RoutesDriverController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

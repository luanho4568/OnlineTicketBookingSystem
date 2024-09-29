using Microsoft.AspNetCore.Mvc;

namespace AdminDriverDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

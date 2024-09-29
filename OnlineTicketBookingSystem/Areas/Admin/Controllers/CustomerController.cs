using Microsoft.AspNetCore.Mvc;

namespace AdminDriverDashboard.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

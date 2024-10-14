using Microsoft.AspNetCore.Mvc;

namespace AdminDriverDashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TripAssignmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

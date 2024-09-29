using Microsoft.AspNetCore.Mvc;

namespace AdminDriverDashboard.Areas.Account.Controllers
{
    [Area("Account")]
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

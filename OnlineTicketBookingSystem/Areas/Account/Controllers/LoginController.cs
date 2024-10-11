using Microsoft.AspNetCore.Mvc;

namespace AdminDriverDashboard.Areas.Account.Controllers
{
    [Area("Account")]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

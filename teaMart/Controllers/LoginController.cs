using Microsoft.AspNetCore.Mvc;

namespace teaMart.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

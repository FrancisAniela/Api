using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class AuthenticateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

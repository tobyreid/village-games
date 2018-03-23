using Microsoft.AspNetCore.Mvc;
using Village.Idiot.Controllers;

namespace Village.Games.Controllers
{
    [SecurityHeaders]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}

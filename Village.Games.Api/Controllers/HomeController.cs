using Microsoft.AspNetCore.Mvc;

namespace Village.Games.Api.Controllers
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

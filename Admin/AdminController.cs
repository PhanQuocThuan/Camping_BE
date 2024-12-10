using Microsoft.AspNetCore.Mvc;

namespace WebCamping.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }
    }
}

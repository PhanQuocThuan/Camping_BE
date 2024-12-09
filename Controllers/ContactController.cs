using Microsoft.AspNetCore.Mvc;

namespace CampingEquipment.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

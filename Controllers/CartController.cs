using Microsoft.AspNetCore.Mvc;

namespace CampingEquipment.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

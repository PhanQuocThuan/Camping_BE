using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebCamping.Models;

namespace CampingEquipment.Controllers
{
    public class HomeController : Controller
    {
        private readonly CampingContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(CampingContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Brands"] = _context.Brands.AsNoTracking()
                 .Include(x => x.Products.OrderBy(y => y.Name))
                 .OrderBy(x => x.BRA_ID).ToList();

            ViewData["HostProducts"] = _context.Products.AsNoTracking()
                .Include(x => x.Brand)
                .OrderBy(x => x.Price).ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErroView { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}

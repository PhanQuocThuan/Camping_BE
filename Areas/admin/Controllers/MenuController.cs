using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCamping.Models;
using WebCamping.Utils;
namespace WebCamping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly CampingContext _context;

        public MenuController(CampingContext context)
        {
            _context = context;
        }

        // GET: Admin/Menu
        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                var allMenus = await _context.Menus.ToListAsync();
                return View(allMenus);
            }
            var menus = await _context.Menus
                                   .FromSqlRaw("SELECT * FROM Menus WHERE Title LIKE {0}", "%" + searchQuery + "%")
                                   .ToListAsync();

            return View(menus);
        }

        // GET: Admin/Menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.MEN_ID == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Admin/Menu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Menu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Menu menu)
        {
            if (ModelState.IsValid)
            {
                var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                var userName = userInfo != null ? userInfo.Username : "";
                menu.UpdatedDate = DateTime.Now;
                menu.UpdatedBy = userName;
                menu.CreatedDate = DateTime.Now;
                menu.CreatedBy = userName;
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        // GET: Admin/Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        // POST: Admin/Menu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Menu menu)
        {
            if (id != menu.MEN_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                    var userName = userInfo != null ? userInfo.Username : "";
                    menu.UpdatedDate = DateTime.Now;
                    menu.UpdatedBy = userName;
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.MEN_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        // GET: Admin/Menu/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menu = await _context.Menus
        //        .FirstOrDefaultAsync(m => m.MEN_ID == id);
        //    if (menu == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menu);
        //}

        // POST: Admin/Menu/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.MEN_ID == id);
        }
    }
}

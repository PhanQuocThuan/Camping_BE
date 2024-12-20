using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCamping.Models;
using WebCamping.Utils;

namespace WebCamping.Areas.admin.Controllers
{
    [Area("admin")]
    public class CategoryController : Controller
    {
        private readonly CampingContext _context;

        public CategoryController(CampingContext context)
        {
            _context = context;
        }

        // GET: admin/Category
        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                var allCategories = await _context.Categories.ToListAsync();
                return View(allCategories);
            }
            var categories = await _context.Categories
                                   .FromSqlRaw("SELECT * FROM Categories WHERE Name LIKE {0}", "%" + searchQuery + "%")
                                   .ToListAsync();

            return View(categories);
        }

        // GET: admin/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CAT_ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: admin/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Category category)
        {
            if (ModelState.IsValid)
            {
                var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                var userName = userInfo != null ? userInfo.Username : "";
                category.UpdatedDate = DateTime.Now;
                category.UpdatedBy = userName;
                category.CreatedDate = DateTime.Now;
                category.CreatedBy = userName;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: admin/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Category category)
        {
            if (id != category.CAT_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                    var userName = userInfo != null ? userInfo.Username : "";
                    category.UpdatedDate = DateTime.Now;
                    category.UpdatedBy = userName;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CAT_ID))
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
            return View(category);
        }

        // GET: admin/Category/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var category = await _context.Categories
        //        .FirstOrDefaultAsync(m => m.CAT_ID == id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}

        // POST: admin/Category/Delete/5
        // POST: admin/Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }


        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CAT_ID == id);
        }
    }
}

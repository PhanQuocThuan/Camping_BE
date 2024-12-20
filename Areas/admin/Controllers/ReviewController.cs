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
    public class ReviewController : Controller
    {
        private readonly CampingContext _context;

        public ReviewController(CampingContext context)
        {
            _context = context;
        }

        // GET: Admin/Review
        public async Task<IActionResult> Index(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                var allReviews = await _context.Reviews
                                                .Include(p => p.Product)
                                                .Include(p => p.User)
                                                .ToListAsync();
                return View(allReviews);
            }
            //truy vấn dữ liệu từ search 
            var reviews = await _context.Reviews
                                         .FromSqlRaw(@"SELECT * FROM Reviews WHERE UserName LIKE {0}", "%" + searchQuery + "%")
                                         .ToListAsync();

            // nạp thông tin product với user
            foreach (var review in reviews)
            {
                await _context.Entry(review).Reference(p => p.Product).LoadAsync();
                await _context.Entry(review).Reference(p => p.User).LoadAsync();
            }
            return View(reviews);
        }

        // GET: Admin/Review/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.REV_ID == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Admin/Review/Create
        public IActionResult Create()
        {
            ViewData["PRO_ID"] = new SelectList(_context.Products, "PRO_ID", "Name");
            ViewData["USE_ID"] = new SelectList(_context.Users, "USE_ID", "UserName");
            return View();
        }

        // POST: Admin/Review/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Review review)
        {
            if (ModelState.IsValid)
            {
                var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                var userName = userInfo != null ? userInfo.Username : "";
                review.UpdatedDate = DateTime.Now;
                review.UpdatedBy = userName;
                review.CreatedDate = DateTime.Now;
                review.CreatedBy = userName;
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PRO_ID"] = new SelectList(_context.Products, "PRO_ID", "Name", review.PRO_ID);
            ViewData["USE_ID"] = new SelectList(_context.Users, "USE_ID", "UserName", review.USE_ID);
            return View(review);
        }

        // GET: Admin/Review/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["PRO_ID"] = new SelectList(_context.Products, "PRO_ID", "Name", review.PRO_ID);
            ViewData["USE_ID"] = new SelectList(_context.Users, "USE_ID", "UserName", review.USE_ID);
            return View(review);
        }

        // POST: Admin/Review/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Review review)
        {
            if (id != review.REV_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                    var userName = userInfo != null ? userInfo.Username : "";
                    review.UpdatedDate = DateTime.Now;
                    review.UpdatedBy = userName;
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.REV_ID))
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
            ViewData["PRO_ID"] = new SelectList(_context.Products, "PRO_ID", "Name", review.PRO_ID);
            ViewData["USE_ID"] = new SelectList(_context.Users, "USE_ID", "UserName", review.USE_ID);
            return View(review);
        }

        // GET: Admin/Review/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var review = await _context.Reviews
        //        .Include(r => r.Product)
        //        .Include(r => r.User)
        //        .FirstOrDefaultAsync(m => m.REV_ID == id);
        //    if (review == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(review);
        //}

        // POST: Admin/Review/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.REV_ID == id);
        }
    }
}

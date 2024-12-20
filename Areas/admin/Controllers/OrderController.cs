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
    public class OrderController : Controller
    {
        private readonly CampingContext _context;

        public OrderController(CampingContext context)
        {
            _context = context;
        }

        // GET: Admin/Order
        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                var allOrders = await _context.Orders.ToListAsync();
                return View(allOrders);
            }
            var orders = await _context.Orders
                                   .FromSqlRaw("SELECT * FROM Orders WHERE CustomerName LIKE {0}", "%" + searchQuery + "%")
                                   .ToListAsync();

            return View(orders);
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.ORD_ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Admin/Order/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Order order)
        {
            if (ModelState.IsValid)
            {
                var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                var userName = userInfo != null ? userInfo.Username : "";
                order.UpdatedDate = DateTime.Now;
                order.UpdatedBy = userName;
                order.CreatedDate = DateTime.Now;
                order.CreatedBy = userName;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Admin/Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Order order)
        {
            if (id != order.ORD_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                    var userName = userInfo != null ? userInfo.Username : "";
                    order.UpdatedDate = DateTime.Now;
                    order.UpdatedBy = userName;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ORD_ID))
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
            return View(order);
        }

        // GET: Admin/Order/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders
        //        .FirstOrDefaultAsync(m => m.ORD_ID == id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}

        // POST: Admin/Order/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ORD_ID == id);
        }
    }
}

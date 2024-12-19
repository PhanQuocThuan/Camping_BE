using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCamping.Models;
using WebCamping.Utils;
using WebCamping.Areas.Admin.DTOs.request;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing.Drawing2D;

namespace WebCamping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly CampingContext _context;
        private readonly IWebHostEnvironment _hostEnv;

        public ProductController(CampingContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {
            var campingContext = _context.Products.Include(p => p.Brand).Include(p => p.Category);
            return View(await campingContext.ToListAsync());
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PRO_ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewData["BRA_ID"] = new SelectList(_context.Brands.OrderBy(x => x.Name), "BRA_ID", "Name");
            ViewData["CAT_ID"] = new SelectList(_context.Categories.OrderBy(x => x.Name), "CAT_ID", "Name");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDTO request)
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            var userName = userInfo != null ? userInfo.Username : "";
            var product = new Product
            {
                PRO_ID = request.PRO_ID,
                CAT_ID = request.CAT_ID,
                BRA_ID = request.BRA_ID,
                Intro = request.Intro,
                Name = request.Name,
                Price = request.Price,
                DiscountPrice = request.DiscountPrice,
                Rate = request.Rate,
                Description = request.Description,
            };
            if (ModelState.IsValid)
            {
                string? newImageFileName = null;
                if (request.Avatar != null)
                {
                    var extension = Path.GetExtension(request.Avatar.FileName);
                    newImageFileName = $"{Guid.NewGuid().ToString()}{extension}";
                    var filePath = Path.Combine(_hostEnv.WebRootPath, "data", "products", newImageFileName);
                    request.Avatar.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                if (newImageFileName != null) product.Avatar = newImageFileName;
                product.UpdatedDate = DateTime.Now;
                product.UpdatedBy = userName;
                product.CreatedDate = DateTime.Now;
                product.CreatedBy = userName;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BRA_ID"] = new SelectList(_context.Brands, "BRA_ID", "Name", product.BRA_ID);
            ViewData["CAT_ID"] = new SelectList(_context.Categories, "CAT_ID", "Name", product.CAT_ID);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BRA_ID"] = new SelectList(_context.Brands, "BRA_ID", "Name", product.BRA_ID);
            ViewData["CAT_ID"] = new SelectList(_context.Categories, "CAT_ID", "Name", product.CAT_ID);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] ProductDTO request)
        {
            if (id != request.PRO_ID)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                var userName = userInfo != null ? userInfo.Username : "";
                try
                {
                    product.CAT_ID = request.CAT_ID;
                    product.BRA_ID = request.BRA_ID;
                    product.Intro = request.Intro;
                    product.Name = request.Name;
                    product.Price = request.Price;
                    product.DiscountPrice = request.DiscountPrice;
                    product.Rate = request.Rate;
                    product.Description = request.Description;
                    if (request.Avatar != null)
                    {
                        if (!string.IsNullOrEmpty(product.Avatar))
                        {
                            var oldImagePath = Path.Combine(_hostEnv.WebRootPath, "data", "products", product.Avatar);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        var extension = Path.GetExtension(request.Avatar.FileName);
                        var newImageFileName = $"{Guid.NewGuid().ToString()}{extension}";
                        var filePath = Path.Combine(_hostEnv.WebRootPath, "data", "products", newImageFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await request.Avatar.CopyToAsync(stream);
                        }

                        product.Avatar = newImageFileName;
                    }
                    product.UpdatedDate = DateTime.Now;
                    product.UpdatedBy = userName;
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(request.PRO_ID))
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
            ViewData["BRA_ID"] = new SelectList(_context.Brands, "BRA_ID", "Name", request.BRA_ID);
            ViewData["CAT_ID"] = new SelectList(_context.Categories, "CAT_ID", "Name", request.CAT_ID);
            return View(request);
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PRO_ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.PRO_ID == id);
        }
    }
}

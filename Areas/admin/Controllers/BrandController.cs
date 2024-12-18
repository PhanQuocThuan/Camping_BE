using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCamping.Areas.Admin.DTOs.request;
using WebCamping.Models;
using WebCamping.Utils;

namespace WebCamping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly CampingContext _context;
        private readonly IWebHostEnvironment _hostEnv;

        public BrandController(CampingContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: Admin/Brand
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brands.ToListAsync());
        }

        // GET: Admin/Brand/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BRA_ID == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Admin/Brand/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brand/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] BrandDTO request)
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            var userName = userInfo != null ? userInfo.Username : "";
            var brand = new Brand
            {
                BRA_ID = request.BRA_ID,
                Name = request.Name,
                Phone = request.Phone,
                Email = request.Email,
                BRA_Address = request.BRA_Address,
            };
            if (ModelState.IsValid)
            {
                string? newImageFileName = null;
                if (request.Avatar != null)
                {
                    var extension = Path.GetExtension(request.Avatar.FileName);
                    newImageFileName = $"{Guid.NewGuid().ToString()}{extension}";
                    var filePath = Path.Combine(_hostEnv.WebRootPath, "data", "brands", newImageFileName);
                    request.Avatar.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                if (newImageFileName != null) brand.Avatar = newImageFileName;
                brand.UpdatedDate = DateTime.Now;
                brand.UpdatedBy = userName;
                brand.CreatedDate = DateTime.Now;
                brand.CreatedBy = userName;
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Admin/Brand/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Admin/Brand/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] BrandDTO request)
        {
            if (id != request.BRA_ID)
            {
                return NotFound();
            }
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                var userName = userInfo != null ? userInfo.Username : "";
                try
                {
                    brand.BRA_ID = request.BRA_ID;
                    brand.Name = request.Name;
                    brand.Phone = request.Phone;
                    brand.Email = request.Email;
                    brand.BRA_Address = request.BRA_Address;
                    if (request.Avatar != null)
                    {
                        if (!string.IsNullOrEmpty(brand.Avatar))
                        {
                            var oldImagePath = Path.Combine(_hostEnv.WebRootPath, "data", "brands", brand.Avatar);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        var extension = Path.GetExtension(request.Avatar.FileName);
                        var newImageFileName = $"{Guid.NewGuid().ToString()}{extension}";
                        var filePath = Path.Combine(_hostEnv.WebRootPath, "data", "brands", newImageFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await request.Avatar.CopyToAsync(stream);
                        }

                        brand.Avatar = newImageFileName;
                    }
                    brand.UpdatedDate = DateTime.Now;
                    brand.UpdatedBy = userName;
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(request.BRA_ID))
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
            return View(request);
        }

        // GET: Admin/Brand/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var brand = await _context.Brand
        //        .FirstOrDefaultAsync(m => m.BRA_ID == id);
        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(brand); 
        //}

        // POST: Admin/Brand/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Brand deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
            return _context.Brand.Any(e => e.BRA_ID == id);
        }
    }
}

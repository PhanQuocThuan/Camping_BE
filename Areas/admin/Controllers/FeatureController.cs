using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCamping.Areas.Admin.DTOs.request;
using WebCamping.Models;
using WebCamping.Utils;

namespace WebCamping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeatureController : Controller
    {
        private readonly CampingContext _context;
        private readonly IWebHostEnvironment _hostEnv;
        public FeatureController(CampingContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: Admin/Feature
        public async Task<IActionResult> Index()
        {
            return View(await _context.Features.ToListAsync());
        }

        // GET: Admin/Feature/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _context.Features
                .FirstOrDefaultAsync(m => m.FEA_ID == id);
            if (feature == null)
            {
                return NotFound();
            }

            return View(feature);
        }

        // GET: Admin/Feature/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Feature/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] FeatureDTO request)
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            var userName = userInfo != null ? userInfo.Username : "";

            var feature = new Feature
            {
                FEA_ID = request.FEA_ID,
                Title = request.Title,
                Subtitle = request.Subtitle,
                DisplayOrder = request.DisplayOrder,
            };
            if (ModelState.IsValid)
            {
                string? newImageFileName = null;
                if (request.Icon != null)
                {
                    var directory = Path.Combine(_hostEnv.WebRootPath, "data", "features");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    var extension = Path.GetExtension(request.Icon.FileName);
                    newImageFileName = $"{Guid.NewGuid().ToString()}{extension}";
                    var filePath = Path.Combine(_hostEnv.WebRootPath, "data", "features", newImageFileName);
                    request.Icon.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                if (newImageFileName != null) feature.Icon = newImageFileName;
                feature.UpdatedDate = DateTime.Now;
                feature.UpdatedBy = userName;
                feature.CreatedDate = DateTime.Now;
                feature.CreatedBy = userName;
                //--------------------
                 _context.Add(feature);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
            }
            return View(feature);
        }

        // GET: Admin/Feature/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _context.Features.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }
            return View(feature);
        }

        // POST: Admin/Feature/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FEA_ID,Icon,Title,Subtitle,DisplayOrder,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Feature feature)
        {
            if (id != feature.FEA_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeatureExists(feature.FEA_ID))
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
            return View(feature);
        }

        // GET: Admin/Feature/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _context.Features
                .FirstOrDefaultAsync(m => m.FEA_ID == id);
            if (feature == null)
            {
                return NotFound();
            }

            return View(feature);
        }

        // POST: Admin/Feature/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feature = await _context.Features.FindAsync(id);
            if (feature != null)
            {
                _context.Features.Remove(feature);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeatureExists(int id)
        {
            return _context.Features.Any(e => e.FEA_ID == id);
        }
    }
}

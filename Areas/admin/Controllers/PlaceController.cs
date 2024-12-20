using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCamping.Areas.Admin.DTOs.request;
using WebCamping.Models;
using WebCamping.Utils;

namespace WebCamping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlaceController : Controller
    {
        private readonly CampingContext _context;
        private readonly IWebHostEnvironment _hostEnv;

        public PlaceController(CampingContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: Admin/Place
        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                var allPlaces = await _context.Places.ToListAsync();
                return View(allPlaces);
            }
            var places = await _context.Places
                               .FromSqlRaw(@"SELECT * FROM Places 
                                         WHERE Name LIKE {0} 
                                         OR Address LIKE {0}", "%" + searchQuery + "%")
                               .ToListAsync();

            return View(places);
        }

        // GET: Admin/Place/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Places
                .FirstOrDefaultAsync(m => m.PLA_ID == id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        // GET: Admin/Place/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Place/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] PlaceDTO request)
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            var userName = userInfo != null ? userInfo.Username : "";
            var place = new Place
            {
                PLA_ID = request.PLA_ID,
                Name = request.Name,
                Address = request.Address,
            };
            if (ModelState.IsValid)
            {
                string? newImageFileName = null;
                if (request.Image != null)
                {
                    var directory = Path.Combine(_hostEnv.WebRootPath, "data", "places");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    var extension = Path.GetExtension(request.Image.FileName);
                    newImageFileName = $"{Guid.NewGuid().ToString()}{extension}";
                    var filePath = Path.Combine(_hostEnv.WebRootPath, "data", "places", newImageFileName);
                    request.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                if (newImageFileName != null) place.Image = newImageFileName;
                place.UpdatedDate = DateTime.Now;
                place.UpdatedBy = userName;
                place.CreatedDate = DateTime.Now;
                place.CreatedBy = userName;
                _context.Add(place);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(place);
        }

        // GET: Admin/Place/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Places.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }
            return View(place);
        }

        // POST: Admin/Place/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] PlaceDTO request)
        {
            if (id != request.PLA_ID)
            {
                return NotFound();
            }
            var place = await _context.Places.FindAsync(id);

            if (ModelState.IsValid)
            {
                var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                var userName = userInfo != null ? userInfo.Username : "";
                try
                {
                    place.PLA_ID = request.PLA_ID;
                    place.Name = request.Name;
                    place.Address = request.Address;
                    if (request.Image != null)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(place.Image))
                            {
                                var oldImagePath = Path.Combine(_hostEnv.WebRootPath, "data", "places", place.Image);
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                            }
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine($"Không thể xóa file: {ex.Message}");
                        }
                        var directory = Path.Combine(_hostEnv.WebRootPath, "data", "places");
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        var extension = Path.GetExtension(request.Image.FileName);
                        var newImageFileName = $"{Guid.NewGuid().ToString()}{extension}";
                        var filePath = Path.Combine(_hostEnv.WebRootPath, "data", "brands", newImageFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await request.Image.CopyToAsync(stream);
                        }

                        place.Image = newImageFileName;
                    }
                    place.UpdatedDate = DateTime.Now;
                    place.UpdatedBy = userName;
                    _context.Update(place);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceExists(request.PLA_ID))
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
            return View(place);
        }

        // GET: Admin/Place/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var place = await _context.Places
        //        .FirstOrDefaultAsync(m => m.PLA_ID == id);
        //    if (place == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(place);
        //}

        // POST: Admin/Place/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var place = await _context.Places.FindAsync(id);
            if (place != null)
            {
                _context.Places.Remove(place);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaceExists(int id)
        {
            return _context.Places.Any(e => e.PLA_ID == id);
        }
    }
}

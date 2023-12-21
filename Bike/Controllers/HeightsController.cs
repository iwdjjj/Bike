using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bike.Data;
using Bike.Models;
using Microsoft.AspNetCore.Authorization;

namespace Bike.Controllers
{
    public class HeightsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HeightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Heights
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
              return _context.Height != null ? 
                          View(await _context.Height.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Height'  is null.");
        }

        // GET: Heights/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Height == null)
            {
                return NotFound();
            }

            var height = await _context.Height
                .FirstOrDefaultAsync(m => m.HeightId == id);
            if (height == null)
            {
                return NotFound();
            }

            return View(height);
        }

        // GET: Heights/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Heights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HeightId,Terrain_height,Complexity,Time")] Height height)
        {
            if (ModelState.IsValid)
            {
                _context.Add(height);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(height);
        }

        // GET: Heights/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Height == null)
            {
                return NotFound();
            }

            var height = await _context.Height.FindAsync(id);
            if (height == null)
            {
                return NotFound();
            }
            return View(height);
        }

        // POST: Heights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HeightId,Terrain_height,Complexity,Time")] Height height)
        {
            if (id != height.HeightId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(height);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeightExists(height.HeightId))
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
            return View(height);
        }

        // GET: Heights/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Height == null)
            {
                return NotFound();
            }

            var height = await _context.Height
                .FirstOrDefaultAsync(m => m.HeightId == id);
            if (height == null)
            {
                return NotFound();
            }

            return View(height);
        }

        // POST: Heights/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Height == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Height'  is null.");
            }
            var height = await _context.Height.FindAsync(id);
            if (height != null)
            {
                _context.Height.Remove(height);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HeightExists(int id)
        {
          return (_context.Height?.Any(e => e.HeightId == id)).GetValueOrDefault();
        }
    }
}

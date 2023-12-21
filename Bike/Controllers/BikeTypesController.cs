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
    public class BikeTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BikeTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BikeTypes
        public async Task<IActionResult> Index()
        {
              return _context.BikeType != null ? 
                          View(await _context.BikeType.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.BikeType'  is null.");
        }

        // GET: BikeTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BikeType == null)
            {
                return NotFound();
            }

            var bikeType = await _context.BikeType
                .FirstOrDefaultAsync(m => m.BikeTypeId == id);
            if (bikeType == null)
            {
                return NotFound();
            }

            return View(bikeType);
        }

        // GET: BikeTypes/Create
        [Authorize(Roles = "Administrator, Guest")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BikeTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator, Guest")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BikeTypeId,Name,Complexity,Speed,Time")] BikeType bikeType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bikeType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bikeType);
        }

        // GET: BikeTypes/Edit/5
        [Authorize(Roles = "Administrator, Guest")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BikeType == null)
            {
                return NotFound();
            }

            var bikeType = await _context.BikeType.FindAsync(id);
            if (bikeType == null)
            {
                return NotFound();
            }
            return View(bikeType);
        }

        // POST: BikeTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator, Guest")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BikeTypeId,Name,Complexity,Speed,Time")] BikeType bikeType)
        {
            if (id != bikeType.BikeTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bikeType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BikeTypeExists(bikeType.BikeTypeId))
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
            return View(bikeType);
        }

        // GET: BikeTypes/Delete/5
        [Authorize(Roles = "Administrator, Guest")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BikeType == null)
            {
                return NotFound();
            }

            var bikeType = await _context.BikeType
                .FirstOrDefaultAsync(m => m.BikeTypeId == id);
            if (bikeType == null)
            {
                return NotFound();
            }

            return View(bikeType);
        }

        // POST: BikeTypes/Delete/5
        [Authorize(Roles = "Administrator, Guest")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BikeType == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BikeType'  is null.");
            }
            var bikeType = await _context.BikeType.FindAsync(id);
            if (bikeType != null)
            {
                _context.BikeType.Remove(bikeType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BikeTypeExists(int id)
        {
          return (_context.BikeType?.Any(e => e.BikeTypeId == id)).GetValueOrDefault();
        }
    }
}

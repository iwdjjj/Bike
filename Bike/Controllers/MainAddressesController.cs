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
    public class MainAddressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MainAddressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MainAddresses
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MainAddress.Include(m => m.Height);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MainAddresses/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MainAddress == null)
            {
                return NotFound();
            }

            var mainAddress = await _context.MainAddress
                .Include(m => m.Height)
                .FirstOrDefaultAsync(m => m.MainAddressId == id);
            if (mainAddress == null)
            {
                return NotFound();
            }

            return View(mainAddress);
        }

        // GET: MainAddresses/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["HeightId"] = new SelectList(_context.Height, "HeightId", "Terrain_height");
            return View();
        }

        // POST: MainAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MainAddressId,Country,State,City,Street,House,HeightId")] MainAddress mainAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mainAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HeightId"] = new SelectList(_context.Height, "HeightId", "Terrain_height", mainAddress.HeightId);
            return View(mainAddress);
        }

        // GET: MainAddresses/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MainAddress == null)
            {
                return NotFound();
            }

            var mainAddress = await _context.MainAddress.FindAsync(id);
            if (mainAddress == null)
            {
                return NotFound();
            }
            ViewData["HeightId"] = new SelectList(_context.Height, "HeightId", "Terrain_height", mainAddress.HeightId);
            return View(mainAddress);
        }

        // POST: MainAddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MainAddressId,Country,State,City,Street,House,HeightId")] MainAddress mainAddress)
        {
            if (id != mainAddress.MainAddressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mainAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MainAddressExists(mainAddress.MainAddressId))
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
            ViewData["HeightId"] = new SelectList(_context.Height, "HeightId", "Terrain_height", mainAddress.HeightId);
            return View(mainAddress);
        }

        // GET: MainAddresses/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MainAddress == null)
            {
                return NotFound();
            }

            var mainAddress = await _context.MainAddress
                .Include(m => m.Height)
                .FirstOrDefaultAsync(m => m.MainAddressId == id);
            if (mainAddress == null)
            {
                return NotFound();
            }

            return View(mainAddress);
        }

        // POST: MainAddresses/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MainAddress == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MainAddress'  is null.");
            }
            var mainAddress = await _context.MainAddress.FindAsync(id);
            if (mainAddress != null)
            {
                _context.MainAddress.Remove(mainAddress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MainAddressExists(int id)
        {
          return (_context.MainAddress?.Any(e => e.MainAddressId == id)).GetValueOrDefault();
        }
    }
}

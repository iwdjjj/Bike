using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bike.Data;
using Bike.Models;
using Route = Bike.Models.Route;
using Microsoft.AspNetCore.Authorization;

namespace Bike.Controllers
{
    public class RoutesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoutesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Routes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Routes.Include(r => r.Address1).Include(r => r.Address2).Include(r => r.BikeType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Routes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Routes == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.Address1)
                .Include(r => r.Address2)
                .Include(r => r.BikeType)
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // GET: Routes/Create
        [Authorize(Roles = "Administrator, Guest")]
        public IActionResult Create()
        {
            ViewData["AddressId1"] = new SelectList(_context.Address, "AddressId", "FullAddress");
            ViewData["AddressId2"] = new SelectList(_context.Address, "AddressId", "FullAddress");
            ViewData["BikeTypeId"] = new SelectList(_context.BikeType, "BikeTypeId", "Name");
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator, Guest")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RouteId,AddressId1,AddressId2,BikeTypeId,Time,TimeResult")] Route route)
        {
            if (ModelState.IsValid)
            {
                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId1"] = new SelectList(_context.Address, "AddressId", "FullAddress", route.AddressId1);
            ViewData["AddressId2"] = new SelectList(_context.Address, "AddressId", "FullAddress", route.AddressId2);
            ViewData["BikeTypeId"] = new SelectList(_context.BikeType, "BikeTypeId", "Name", route.BikeTypeId);
            return View(route);
        }

        // GET: Routes/Edit/5
        [Authorize(Roles = "Administrator, Guest")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Routes == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            ViewData["AddressId1"] = new SelectList(_context.Address, "AddressId", "FullAddress", route.AddressId1);
            ViewData["AddressId2"] = new SelectList(_context.Address, "AddressId", "FullAddress", route.AddressId2);
            ViewData["BikeTypeId"] = new SelectList(_context.BikeType, "BikeTypeId", "Name", route.BikeTypeId);
            return View(route);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator, Guest")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RouteId,AddressId1,AddressId2,BikeTypeId,Time,TimeResult")] Route route)
        {
            if (id != route.RouteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(route);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(route.RouteId))
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
            ViewData["AddressId1"] = new SelectList(_context.Address, "AddressId", "FullAddress", route.AddressId1);
            ViewData["AddressId2"] = new SelectList(_context.Address, "AddressId", "FullAddress", route.AddressId2);
            ViewData["BikeTypeId"] = new SelectList(_context.BikeType, "BikeTypeId", "Name", route.BikeTypeId);
            return View(route);
        }

        // GET: Routes/Delete/5
        [Authorize(Roles = "Administrator, Guest")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Routes == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.Address1)
                .Include(r => r.Address2)
                .Include(r => r.BikeType)
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // POST: Routes/Delete/5
        [Authorize(Roles = "Administrator, Guest")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Routes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Routes'  is null.");
            }
            var route = await _context.Routes.FindAsync(id);
            if (route != null)
            {
                _context.Routes.Remove(route);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouteExists(int id)
        {
          return (_context.Routes?.Any(e => e.RouteId == id)).GetValueOrDefault();
        }
    }
}

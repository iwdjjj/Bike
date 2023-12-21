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
    public class BikeRoutesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BikeRoutesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BikeRoutes
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Name"] = _context.BikeType.Select(d => new { id = d.BikeTypeId, Name = d.Name }).FirstOrDefault(d => d.id == id).Name;
            ViewData["IdBikeType"] = _context.BikeType.Select(d => new { id = d.BikeTypeId, Name = d.Name }).FirstOrDefault(d => d.id == id).id;

            var applicationDbContext = _context.Routes.Where(d => d.BikeTypeId == id).Include(r => r.Address1).Include(r => r.Address2).Include(r => r.BikeType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BikeRoutes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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

        // GET: BikeRoutes/Create
        [Authorize(Roles = "Administrator, Guest")]
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["AddressId1"] = new SelectList(_context.Address, "AddressId", "FullAddress");
            ViewData["AddressId2"] = new SelectList(_context.Address, "AddressId", "FullAddress");
            ViewData["BikeTypeId"] = id;
            return View();
        }

        // POST: BikeRoutes/Create
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
                return RedirectToAction(nameof(Index), new { id = route.BikeTypeId });
            }
            ViewData["AddressId1"] = new SelectList(_context.Address, "AddressId", "FullAddress", route.AddressId1);
            ViewData["AddressId2"] = new SelectList(_context.Address, "AddressId", "FullAddress", route.AddressId2);
            ViewData["BikeTypeId"] = route.BikeTypeId;
            return View(route);
        }

        // GET: BikeRoutes/Edit/5
        [Authorize(Roles = "Administrator, Guest")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
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
            ViewData["IdBikeType"] = route.BikeTypeId;
            return View(route);
        }

        // POST: BikeRoutes/Edit/5
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
                return RedirectToAction(nameof(Index), new { id = route.BikeTypeId });
            }
            ViewData["AddressId1"] = new SelectList(_context.Address, "AddressId", "FullAddress", route.AddressId1);
            ViewData["AddressId2"] = new SelectList(_context.Address, "AddressId", "FullAddress", route.AddressId2);
            ViewData["BikeTypeId"] = new SelectList(_context.BikeType, "BikeTypeId", "Name", route.BikeTypeId);
            ViewData["IdBikeType"] = route.BikeTypeId;
            return View(route);
        }

        // GET: BikeRoutes/Delete/5
        [Authorize(Roles = "Administrator, Guest")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
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

        // POST: BikeRoutes/Delete/5
        [Authorize(Roles = "Administrator, Guest")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = route.BikeTypeId });
        }

        private bool RouteExists(int id)
        {
          return (_context.Routes?.Any(e => e.RouteId == id)).GetValueOrDefault();
        }
    }
}

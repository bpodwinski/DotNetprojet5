using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoituresV2.Data;
using ExpressVoituresV2.Models;

namespace ExpressVoituresV2
{
    public class Vehicles2Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public Vehicles2Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles2
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vehicle.Include(v => v.Brand).Include(v => v.Model).Include(v => v.TrimLevel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vehicles2/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Brand)
                .Include(v => v.Model)
                .Include(v => v.TrimLevel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles2/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id");
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Id");
            ViewData["TrimLevelId"] = new SelectList(_context.TrimLevels, "Id", "Id");
            return View();
        }

        // POST: Vehicles2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Vin,Year,PurchaseDate,PurchasePrice,AvailabilityDate,SaleDate,BrandId,ModelId,TrimLevelId,Description,ImagePath,TotalRepairCost")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", vehicle.BrandId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Id", vehicle.ModelId);
            ViewData["TrimLevelId"] = new SelectList(_context.TrimLevels, "Id", "Id", vehicle.TrimLevelId);
            return View(vehicle);
        }

        // GET: Vehicles2/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", vehicle.BrandId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Id", vehicle.ModelId);
            ViewData["TrimLevelId"] = new SelectList(_context.TrimLevels, "Id", "Id", vehicle.TrimLevelId);
            return View(vehicle);
        }

        // POST: Vehicles2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Vin,Year,PurchaseDate,PurchasePrice,AvailabilityDate,SaleDate,BrandId,ModelId,TrimLevelId,Description,ImagePath,TotalRepairCost")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", vehicle.BrandId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Id", vehicle.ModelId);
            ViewData["TrimLevelId"] = new SelectList(_context.TrimLevels, "Id", "Id", vehicle.TrimLevelId);
            return View(vehicle);
        }

        // GET: Vehicles2/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Brand)
                .Include(v => v.Model)
                .Include(v => v.TrimLevel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }
    }
}

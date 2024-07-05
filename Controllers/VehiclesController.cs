using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoituresV2.Data;
using ExpressVoituresV2.Models;
using ExpressVoituresV2.ViewModel;
using Microsoft.IdentityModel.Tokens;

namespace ExpressVoituresV2.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vehicle.Include(v => v.Brand);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsPartial", vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ViewData["BrandList"] = new SelectList(_context.Brands, "Id", "Name");
            return View();
        }

        // POST: Vehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Vin,Year,BrandId,BrandAdd,BrandList,PurchaseDate,PurchasePrice,AvailabilityDate,SalePrice,SaleDate")] VehicleViewModel viewModel)
        {
	        if (!string.IsNullOrEmpty(viewModel.BrandAdd))
	        {
		        var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Name == viewModel.BrandAdd);
		        if (existingBrand == null)
		        {
			        var newBrand = new Brand { Name = viewModel.BrandAdd };
			        _context.Brands.Add(newBrand);
			        await _context.SaveChangesAsync();
			        viewModel.BrandId = newBrand.Id;
		        }
		        else
		        {
			        viewModel.BrandId = existingBrand.Id;
		        }
	        }
	        else if (!string.IsNullOrEmpty(viewModel.BrandList))
	        {
		        viewModel.BrandId = Convert.ToInt32(viewModel.BrandList);
	        }

			if (ModelState.IsValid)
	        {
		        var vehicle = new Vehicle
		        {
			        Vin = viewModel.Vin,
			        Year = viewModel.Year,
			        PurchaseDate = viewModel.PurchaseDate,
			        PurchasePrice = viewModel.PurchasePrice,
			        AvailabilityDate = viewModel.AvailabilityDate,
			        SalePrice = viewModel.SalePrice,
			        SaleDate = viewModel.SaleDate
		        };

				_context.Vehicle.Add(vehicle);
		        await _context.SaveChangesAsync();

				return RedirectToAction(nameof(Index));
	        }

			return View(viewModel);
        }


		// GET: Vehicles/Edit/5
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", vehicle.Brand);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Vin,Year,BrandId,PurchaseDate,PurchasePrice,AvailabilityDate,SalePrice,SaleDate")] Vehicle vehicle)
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", vehicle.Brand);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartial", vehicle);
        }

        // POST: Vehicles/Delete/5
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

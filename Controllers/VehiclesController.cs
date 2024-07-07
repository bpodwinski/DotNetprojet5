using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoituresV2.Data;
using ExpressVoituresV2.Models;
using ExpressVoituresV2.ViewModel;

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
		[HttpGet("/admin/vehicle")]
		public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vehicle
                .Include(v => v.Brand)
                .Include(v => v.Model)
                .Include(v => v.TrimLevel)
                .Include(v => v.Repairs)
                .Select(v => new VehicleViewModel
                {
                    Id = v.Id,
                    Vin = v.Vin,
                    Year = v.Year,
                    Brand = v.Brand,
                    Model = v.Model,
                    TrimLevel = v.TrimLevel,
                    PurchaseDate = v.PurchaseDate,
                    PurchasePrice = v.PurchasePrice,
                    AvailabilityDate = v.AvailabilityDate,
                    SaleDate = v.SaleDate,
                    SalePrice = v.SalePrice,
                    TotalRepairCost = v.Repairs.Any() ? v.Repairs.Sum(r => r.Cost) : (decimal?)null
                });

			return View(await applicationDbContext.ToListAsync());
        }

		// GET: Vehicles/Details/5
		[HttpGet("/admin/vehicle/details/{id}")]
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

            return PartialView("_DetailsPartial", vehicle);
        }

		// GET: Vehicles/Create
		[HttpGet("/admin/vehicle/add")]
		public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            //ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name");
            //ViewData["TrimLevelId"] = new SelectList(_context.TrimLevels, "Id", "Name");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost("/admin/vehicle/add")]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Vin,Year,PurchaseDate,PurchasePrice,AvailabilityDate,SalePrice,SaleDate,BrandId,ModelId,TrimLevelId")] Vehicle vehicle)
        {
            ModelState.Remove("Brand");
            ModelState.Remove("Model");
            ModelState.Remove("TrimLevel");
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", vehicle.BrandId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name", vehicle.ModelId);
            ViewData["TrimLevelId"] = new SelectList(_context.TrimLevels, "Id", "Name", vehicle.TrimLevelId);
            return View(vehicle);
        }

		// GET: VehiclesNew/Edit/5
		[HttpGet("/admin/vehicle/edit{id}")]
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", vehicle.BrandId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name", vehicle.ModelId);
            ViewData["TrimLevelId"] = new SelectList(_context.TrimLevels, "Id", "Name", vehicle.TrimLevelId);
            return View(vehicle);
        }

        // POST: VehiclesNew/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost("/admin/vehicle/edit{id}")]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Vin,Year,PurchaseDate,PurchasePrice,AvailabilityDate,SalePrice,SaleDate,BrandId,ModelId,TrimLevelId")] Vehicle vehicle)
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", vehicle.BrandId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name", vehicle.ModelId);
            ViewData["TrimLevelId"] = new SelectList(_context.TrimLevels, "Id", "Name", vehicle.TrimLevelId);
            return View(vehicle);
        }

		// GET: VehiclesNew/Delete/5
		[HttpGet("/admin/vehicle/delete{id}")]
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

            return PartialView("_DeletePartial", vehicle);
        }

        // POST: VehiclesNew/Delete/5
		[HttpPost("/admin/vehicle/delete{id}"), ActionName("Delete")]
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

        // Action pour obtenir les modèles basés sur BrandId
        public async Task<JsonResult> GetModelsByBrand(int brandId)
        {
            var models = await _context.Models.Where(m => m.BrandId == brandId).ToListAsync();
            return Json(new SelectList(models, "Id", "Name"));
        }

        // Action pour obtenir les niveaux de finition basés sur ModelId
        public async Task<JsonResult> GetTrimLevelsByModel(int modelId)
        {
            var trimLevels = await _context.TrimLevels.Where(t => t.ModelId == modelId).ToListAsync();
            return Json(new SelectList(trimLevels, "Id", "Name"));
        }
    }
}

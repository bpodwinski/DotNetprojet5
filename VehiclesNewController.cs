using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoituresV2.Data;
using ExpressVoituresV2.Models;

namespace ExpressVoituresV2
{
    public class VehiclesNewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesNewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VehiclesNew
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vehicle.Include(v => v.Brand).Include(v => v.Model).Include(v => v.TrimLevel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VehiclesNew/Details/5
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

        // GET: VehiclesNew/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name");
            ViewData["TrimLevelId"] = new SelectList(_context.TrimLevels, "Id", "Name");
            return View();
        }

        // POST: VehiclesNew/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
        [HttpPost]
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

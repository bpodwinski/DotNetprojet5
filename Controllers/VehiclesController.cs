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
                .Include(v => v.Repairs);

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
        public async Task<IActionResult> Create([Bind("Id,Vin,Year,PurchaseDate,PurchasePrice,AvailabilityDate,SalePrice,SaleDate,BrandId,ModelId,TrimLevelId,Description")] Vehicle vehicle, IFormFile ImagePath)
        {

			if (ImagePath != null && ImagePath.Length > 0)
			{
				var extension = Path.GetExtension(ImagePath.FileName);

				var fileName = Path.GetFileNameWithoutExtension(ImagePath.FileName);
				fileName = fileName.Replace(" ", "-");
				fileName = string.Concat(fileName.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));
				fileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{extension}".ToLower();

				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/adverts", fileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await ImagePath.CopyToAsync(fileStream);
				}

				vehicle.ImagePath = "/images/adverts/" + fileName;
			}

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
		[HttpGet("/admin/vehicle/{id}/edit")]
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
		[HttpPost("/admin/vehicle/{id}/edit")]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Vin,Year,PurchaseDate,PurchasePrice,AvailabilityDate,SalePrice,SaleDate,BrandId,ModelId,TrimLevelId")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

			ModelState.Remove("Brand");
			ModelState.Remove("Model");
			ModelState.Remove("TrimLevel");
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
		[HttpGet("/admin/vehicle/{id}/delete")]
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
		[HttpPost("/admin/vehicle/{id}/delete"), ActionName("Delete")]
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

using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;

namespace ExpressVoitures.Controllers
{
	[IgnoreAntiforgeryToken]
	public class VehiclesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public VehiclesController(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Gets the list of vehicles.
		/// </summary>
		/// <returns>A view with the list of vehicles.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
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

		/// <summary>
		/// Gets the details of a specific vehicle.
		/// </summary>
		/// <param name="id">The ID of the vehicle.</param>
		/// <returns>A partial view with the vehicle details.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		[HttpGet("/admin/vehicle/{id}/details")]
		public async Task<IActionResult> Details(int id)
		{
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

		/// <summary>
		/// Shows the create vehicle form.
		/// </summary>
		/// <returns>A view with the create vehicle form.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		[HttpGet("/admin/vehicle/add")]
		public IActionResult Create()
		{
			ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
			return View();
		}

		/// <summary>
		/// Creates a new vehicle.
		/// </summary>
		/// <param name="vehicle">The vehicle to create.</param>
		/// <param name="ImagePath">The image file to upload.</param>
		/// <returns>A redirect to the index view if successful, otherwise the create view.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		[HttpPost("/admin/vehicle/add")]
		public async Task<IActionResult> Create([Bind("Id,Vin,Year,PurchaseDate,PurchasePrice,AvailabilityDate,SaleDate,BrandId,ModelId,TrimLevelId,Description")] Vehicle vehicle, IFormFile ImagePath)
		{
			if (ImagePath != null && ImagePath.Length > 0)
			{
				var extension = Path.GetExtension(ImagePath.FileName);

				var fileName = Path.GetFileNameWithoutExtension(ImagePath.FileName);
				fileName = fileName.Replace(" ", "-");
				fileName = string.Concat(fileName.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));
				fileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{extension}".ToLower();

				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/adverts", fileName);

				using (var image = await SixLabors.ImageSharp.Image.LoadAsync<Rgba32>(ImagePath.OpenReadStream()))
				{
					// Redimensionner l'image à 800x600
					image.Mutate(x => x.Resize(new ResizeOptions
					{
						Size = new Size(800, 600),
						Mode = ResizeMode.Crop
					}));

					await image.SaveAsync(filePath, new JpegEncoder());
				}

				vehicle.ImagePath = "/images/adverts/" + fileName;
			}

			ModelState.Remove("Brand");
			ModelState.Remove("Model");
			ModelState.Remove("TrimLevel");
			ModelState.Remove("ImagePath");
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

		/// <summary>
		/// Shows the edit vehicle form for a specific vehicle.
		/// </summary>
		/// <param name="id">The ID of the vehicle.</param>
		/// <returns>A view with the edit vehicle form.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		[HttpGet("/admin/vehicle/{id}/edit")]
		public async Task<IActionResult> Edit(int id)
		{
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

		/// <summary>
		/// Edits a specific vehicle.
		/// </summary>
		/// <param name="id">The ID of the vehicle.</param>
		/// <param name="vehicle">The updated vehicle information.</param>
		/// <returns>A redirect to the index view if successful, otherwise the edit view.</returns>
		[Authorize]
		[HttpPost("/admin/vehicle/{id}/edit")]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Vin,Year,PurchaseDate,PurchasePrice,AvailabilityDate,SaleDate,BrandId,ModelId,TrimLevelId,Description")] Vehicle vehicle, IFormFile ImagePath)
		{
			if (id != vehicle.Id)
			{
				return NotFound();
			}

			var existingVehicle = await _context.Vehicle.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
			if (existingVehicle == null)
			{
				return NotFound();
			}

			if (ImagePath != null && ImagePath.Length > 0)
			{
				var extension = Path.GetExtension(ImagePath.FileName);

				var fileName = Path.GetFileNameWithoutExtension(ImagePath.FileName);
				fileName = fileName.Replace(" ", "-");
				fileName = string.Concat(fileName.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));
				fileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{extension}".ToLower();

				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/adverts", fileName);

				using (var image = await Image.LoadAsync<Rgba32>(ImagePath.OpenReadStream()))
				{
					var destWidth = 1280;
					var destHeight = 720;

					// Calculate cropping rectangle
					var cropRect = new Rectangle(0, 0, image.Width, image.Height);
					if ((image.Width / (double)image.Height) > (16 / 9.0))
					{
						var newWidth = (int)(image.Height * (16 / 9.0));
						cropRect.X = (image.Width - newWidth) / 2;
						cropRect.Width = newWidth;
					}
					else
					{
						var newHeight = (int)(image.Width * (9 / 16.0));
						cropRect.Y = (image.Height - newHeight) / 2;
						cropRect.Height = newHeight;
					}

					// Crop and resize the image
					image.Mutate(x => x
						.Crop(cropRect)
						.Resize(destWidth, destHeight));

					await image.SaveAsync(filePath, new JpegEncoder());
				}

				vehicle.ImagePath = "/images/adverts/" + fileName;
			}
			else
			{
				vehicle.ImagePath = existingVehicle.ImagePath;
			}

			ModelState.Remove("Brand");
			ModelState.Remove("Model");
			ModelState.Remove("TrimLevel");
			ModelState.Remove("ImagePath");
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

		/// <summary>
		/// Shows the delete vehicle confirmation form for a specific vehicle.
		/// </summary>
		/// <param name="id">The ID of the vehicle.</param>
		/// <returns>A partial view with the delete vehicle confirmation form.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		[HttpGet("/admin/vehicle/{id}/delete")]
		public async Task<IActionResult> Delete(int id)
		{
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

		/// <summary>
		/// Deletes a specific vehicle.
		/// </summary>
		/// <param name="id">The ID of the vehicle.</param>
		/// <returns>A redirect to the index view.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		[HttpPost("/admin/vehicle/{id}/delete"), ActionName("Delete")]
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

		/// <summary>
		/// Checks if a vehicle exists.
		/// </summary>
		/// <param name="id">The ID of the vehicle to check.</param>
		/// <returns>True if the vehicle exists, otherwise false.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		private bool VehicleExists(int id)
		{
			return _context.Vehicle.Any(e => e.Id == id);
		}

		/// <summary>
		/// Gets the models based on BrandId.
		/// </summary>
		/// <param name="brandId">The ID of the brand.</param>
		/// <returns>A JSON result with the list of models.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		public async Task<JsonResult> GetModelsByBrand(int brandId)
		{
			var models = await _context.Models.Where(m => m.BrandId == brandId).ToListAsync();
			return Json(new SelectList(models, "Id", "Name"));
		}

		/// <summary>
		/// Gets the trim levels based on ModelId.
		/// </summary>
		/// <param name="modelId">The ID of the model.</param>
		/// <returns>A JSON result with the list of trim levels.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		public async Task<JsonResult> GetTrimLevelsByModel(int modelId)
		{
			var trimLevels = await _context.TrimLevels.Where(t => t.ModelId == modelId).ToListAsync();
			return Json(new SelectList(trimLevels, "Id", "Name"));
		}
	}
}

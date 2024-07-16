using System.Drawing.Drawing2D;
using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
	[IgnoreAntiforgeryToken]
	public class BrandsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public BrandsController(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Gets the list of all brands.
		/// </summary>
		/// <returns>A view with the list of brands.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.Brands.ToListAsync());
		}

		/// <summary>
		/// Gets the details of a specific brand.
		/// </summary>
		/// <param name="id">The ID of the brand.</param>
		/// <returns>A partial view with the brand details.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var brand = await _context.Brands
				.FirstOrDefaultAsync(m => m.Id == id);
			if (brand == null)
			{
				return NotFound();
			}

			return PartialView("_DetailsPartial", brand);
		}

		/// <summary>
		/// Shows the create brand form.
		/// </summary>
		/// <returns>A partial view with the create brand form.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		public IActionResult Create()
		{
			return PartialView("_CreatePartial");
		}

		/// <summary>
		/// Creates a new brand.
		/// </summary>
		/// <param name="brand">The brand to create.</param>
		/// <returns>A redirect to the index view if successful, otherwise the create partial view.</returns>
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Create([Bind("Id,Name")] Brand brand)
		{
			ModelState.Remove("Models");

			if (string.IsNullOrWhiteSpace(brand.Name))
			{
				ModelState.AddModelError("Name", "Le nom de la marque est requis.");
				return PartialView("_CreatePartial", brand);
			}

			var existingBrand = await _context.Brands
				.FirstOrDefaultAsync(b => b.Name.ToLower() == brand.Name.ToLower());

			if (existingBrand != null)
			{
				ModelState.AddModelError("Name", "Une marque avec ce nom existe déjà.");
			}

			if (ModelState.IsValid)
			{
				_context.Add(brand);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return PartialView("_CreatePartial", brand);
		}

		/// <summary>
		/// Shows the edit brand form.
		/// </summary>
		/// <param name="id">The ID of the brand to edit.</param>
		/// <returns>A partial view with the edit brand form.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var brand = await _context.Brands.FindAsync(id);
			if (brand == null)
			{
				return NotFound();
			}
			return PartialView("_EditPartial", brand);
		}

		/// <summary>
		/// Edits a specific brand.
		/// </summary>
		/// <param name="id">The ID of the brand to edit.</param>
		/// <param name="brand">The updated brand information.</param>
		/// <returns>A redirect to the index view if successful, otherwise the edit partial view.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		[HttpPost]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Brand brand)
		{
			if (id != brand.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(brand);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!BrandExists(brand.Id))
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
			return PartialView("_EditPartial", brand);
		}

		/// <summary>
		/// Shows the delete brand confirmation form.
		/// </summary>
		/// <param name="id">The ID of the brand to delete.</param>
		/// <returns>A partial view with the delete brand confirmation form.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var brand = await _context.Brands
				.FirstOrDefaultAsync(m => m.Id == id);
			if (brand == null)
			{
				return NotFound();
			}

			return PartialView("_DeletePartial", brand);
		}

		/// <summary>
		/// Deletes a specific brand.
		/// </summary>
		/// <param name="id">The ID of the brand to delete.</param>
		/// <returns>A redirect to the index view.</returns>
		[Authorize]
		[IgnoreAntiforgeryToken]
		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var brand = await _context.Brands.FindAsync(id);
                if (brand != null)
                {
                    _context.Brands.Remove(brand);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorDeleteMessage"] = "Cette marque ne peut pas être supprimée car elle est utilisée pour un ou plusieurs véhicules, ou elle contient des modèles et finitions.\n Veuillez supprimer ces éléments avant de procéder.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Checks if a brand exists.
        /// </summary>
        /// <param name="id">The ID of the brand to check.</param>
        /// <returns>True if the brand exists, otherwise false.</returns>
        private bool BrandExists(int id)
		{
			return _context.Brands.Any(e => e.Id == id);
		}
	}
}

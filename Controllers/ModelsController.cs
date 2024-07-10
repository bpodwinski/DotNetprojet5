using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
	public class ModelsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ModelsController(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Gets the list of all models.
		/// </summary>
		/// <returns>A view with the list of models.</returns>
		[Authorize]
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Models.Include(m => m.Brand);
			return View(await applicationDbContext.ToListAsync());
		}

		/// <summary>
		/// Gets the details of a specific model.
		/// </summary>
		/// <param name="id">The ID of the model.</param>
		/// <returns>A partial view with the model details.</returns>
		[Authorize]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var model = await _context.Models
				.Include(m => m.Brand)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (model == null)
			{
				return NotFound();
			}

			return PartialView("_DetailsPartial", model);
		}

		/// <summary>
		/// Shows the create model form.
		/// </summary>
		/// <returns>A partial view with the create model form.</returns>
		[Authorize]
		public IActionResult Create()
		{
			ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
			return PartialView("_CreatePartial");
		}

		/// <summary>
		/// Creates a new model.
		/// </summary>
		/// <param name="model">The model to create.</param>
		/// <returns>A redirect to the index view if successful, otherwise the create partial view.</returns>
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,BrandId")] Model model)
		{
			ModelState.Remove("Brand");
			ModelState.Remove("TrimLevels");
			if (ModelState.IsValid)
			{
				_context.Add(model);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", model.BrandId);
			return PartialView("_CreatePartial");
		}

		/// <summary>
		/// Shows the edit model form.
		/// </summary>
		/// <param name="id">The ID of the model to edit.</param>
		/// <returns>A partial view with the edit model form.</returns>
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var model = await _context.Models.FindAsync(id);
			if (model == null)
			{
				return NotFound();
			}
			ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", model.BrandId);
			return PartialView("_EditPartial", model);
		}

		/// <summary>
		/// Edits a specific model.
		/// </summary>
		/// <param name="id">The ID of the model to edit.</param>
		/// <param name="model">The updated model information.</param>
		/// <returns>A redirect to the index view if successful, otherwise the edit partial view.</returns>
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BrandId")] Model model)
		{
			if (id != model.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(model);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ModelExists(model.Id))
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
			ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", model.BrandId);
			return PartialView("_EditPartial", model);
		}

		/// <summary>
		/// Shows the delete model confirmation form.
		/// </summary>
		/// <param name="id">The ID of the model to delete.</param>
		/// <returns>A partial view with the delete model confirmation form.</returns>
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var model = await _context.Models
				.Include(m => m.Brand)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (model == null)
			{
				return NotFound();
			}

			return PartialView("_DeletePartial", model);
		}

		/// <summary>
		/// Deletes a specific model.
		/// </summary>
		/// <param name="id">The ID of the model to delete.</param>
		/// <returns>A redirect to the index view.</returns>
		[Authorize]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var model = await _context.Models.FindAsync(id);
			if (model != null)
			{
				_context.Models.Remove(model);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		/// <summary>
		/// Checks if a model exists.
		/// </summary>
		/// <param name="id">The ID of the model to check.</param>
		/// <returns>True if the model exists, otherwise false.</returns>
		private bool ModelExists(int id)
		{
			return _context.Models.Any(e => e.Id == id);
		}
	}
}

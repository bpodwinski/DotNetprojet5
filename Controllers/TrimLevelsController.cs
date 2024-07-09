using ExpressVoituresV2.Data;
using ExpressVoituresV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoituresV2
{
	public class TrimLevelsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public TrimLevelsController(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Gets the list of all trim levels.
		/// </summary>
		/// <returns>A view with the list of trim levels.</returns>
		[Authorize]
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.TrimLevels
				.Include(t => t.Model)
				.ThenInclude(m => m.Brand);

			return View(await applicationDbContext.ToListAsync());
		}

		/// <summary>
		/// Gets the details of a specific trim level.
		/// </summary>
		/// <param name="id">The ID of the trim level.</param>
		/// <returns>A partial view with the trim level details.</returns>
		[Authorize]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var trimLevel = await _context.TrimLevels
				.Include(t => t.Model)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (trimLevel == null)
			{
				return NotFound();
			}

			return PartialView("_DetailsPartial", trimLevel);
		}

		/// <summary>
		/// Shows the create trim level form.
		/// </summary>
		/// <returns>A partial view with the create trim level form.</returns>
		[Authorize]
		public IActionResult Create()
		{
			ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name");
			return PartialView("_CreatePartial");
		}

		/// <summary>
		/// Creates a new trim level.
		/// </summary>
		/// <param name="trimLevel">The trim level to create.</param>
		/// <returns>A redirect to the index view if successful, otherwise the create partial view.</returns>
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,ModelId")] TrimLevel trimLevel)
		{
			ModelState.Remove("Brand");
			ModelState.Remove("Model");
			if (ModelState.IsValid)
			{
				_context.Add(trimLevel);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name", trimLevel.ModelId);
			return PartialView("_CreatePartial");
		}

		/// <summary>
		/// Shows the edit trim level form.
		/// </summary>
		/// <param name="id">The ID of the trim level to edit.</param>
		/// <returns>A partial view with the edit trim level form.</returns>
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var trimLevel = await _context.TrimLevels.FindAsync(id);
			if (trimLevel == null)
			{
				return NotFound();
			}
			ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name", trimLevel.ModelId);
			return PartialView("_EditPartial", trimLevel);
		}

		/// <summary>
		/// Edits a specific trim level.
		/// </summary>
		/// <param name="id">The ID of the trim level to edit.</param>
		/// <param name="trimLevel">The updated trim level information.</param>
		/// <returns>A redirect to the index view if successful, otherwise the edit partial view.</returns>
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ModelId")] TrimLevel trimLevel)
		{
			if (id != trimLevel.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(trimLevel);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TrimLevelExists(trimLevel.Id))
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
			ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name", trimLevel.ModelId);
			return PartialView("_EditPartial", trimLevel);
		}

		/// <summary>
		/// Shows the delete trim level confirmation form.
		/// </summary>
		/// <param name="id">The ID of the trim level to delete.</param>
		/// <returns>A partial view with the delete trim level confirmation form.</returns>
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var trimLevel = await _context.TrimLevels
				.Include(t => t.Model)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (trimLevel == null)
			{
				return NotFound();
			}

			return PartialView("_DeletePartial", trimLevel);
		}

		/// <summary>
		/// Deletes a specific trim level.
		/// </summary>
		/// <param name="id">The ID of the trim level to delete.</param>
		/// <returns>A redirect to the index view.</returns>
		[Authorize]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var trimLevel = await _context.TrimLevels.FindAsync(id);
			if (trimLevel != null)
			{
				_context.TrimLevels.Remove(trimLevel);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		/// <summary>
		/// Checks if a trim level exists.
		/// </summary>
		/// <param name="id">The ID of the trim level to check.</param>
		/// <returns>True if the trim level exists, otherwise false.</returns>
		private bool TrimLevelExists(int id)
		{
			return _context.TrimLevels.Any(e => e.Id == id);
		}
	}
}

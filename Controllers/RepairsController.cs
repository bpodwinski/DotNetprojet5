using ExpressVoituresV2.Data;
using ExpressVoituresV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoituresV2.Controllers
{
	public class RepairsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public RepairsController(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Gets the list of repairs for a specific vehicle.
		/// </summary>
		/// <param name="vehicleId">The ID of the vehicle.</param>
		/// <returns>A view with the list of repairs.</returns>
		[Authorize]
		[HttpGet("/admin/vehicle/{vehicleId}/repair")]
		public async Task<IActionResult> Index(int vehicleId)
		{
			var vehicle = await _context.Vehicle
				.Include(v => v.Repairs)
				.FirstOrDefaultAsync(v => v.Id == vehicleId);

			if (vehicle == null)
			{
				return NotFound();
			}

			var totalRepairCost = vehicle.Repairs?.Sum(r => r.Cost) ?? 0;

			var repairs = await _context.Repair
				.Where(r => r.VehicleId == vehicleId)
				.ToListAsync();

			ViewData["Vehicle"] = vehicle;
			ViewData["TotalRepairCost"] = totalRepairCost;

			return View(repairs);
		}

		/// <summary>
		/// Shows the create repair form for a specific vehicle.
		/// </summary>
		/// <param name="vehicleId">The ID of the vehicle.</param>
		/// <returns>A view with the create repair form.</returns>
		[Authorize]
		[HttpGet("/admin/vehicle/{vehicleId}/repair/add")]
		public IActionResult Create(int vehicleId)
		{
			var vehicle = _context.Vehicle.Find(vehicleId);

			if (vehicle == null)
			{
				return NotFound();
			}

			ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", vehicleId);

			return View(new Repair { VehicleId = vehicleId });
		}

		/// <summary>
		/// Creates a new repair for a specific vehicle.
		/// </summary>
		/// <param name="vehicleId">The ID of the vehicle.</param>
		/// <param name="repair">The repair to create.</param>
		/// <returns>A redirect to the index view if successful, otherwise the create view.</returns>
		[Authorize]
		[HttpPost("/admin/vehicle/{vehicleId}/repair/add")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(int vehicleId, [Bind("Id,Name,Cost,VehicleId")] Repair repair)
		{
			ModelState.Remove("Vehicle");
			if (ModelState.IsValid)
			{
				_context.Add(repair);
				await _context.SaveChangesAsync();

				return RedirectToAction("Index", new { vehicleId });
			}

			ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", repair.VehicleId);

			return View(repair);
		}

		/// <summary>
		/// Shows the edit repair form for a specific repair.
		/// </summary>
		/// <param name="vehicleId">The ID of the vehicle.</param>
		/// <param name="id">The ID of the repair.</param>
		/// <returns>A view with the edit repair form.</returns>
		[Authorize]
		[HttpGet("/admin/vehicle/{vehicleId}/repair/edit/{id}")]
		public async Task<IActionResult> Edit(int vehicleId, int id)
		{
			var repair = await _context.Repair.FindAsync(id);

			if (repair == null)
			{
				return NotFound();
			}

			ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", repair.VehicleId);

			return View(repair);
		}

		/// <summary>
		/// Edits a specific repair.
		/// </summary>
		/// <param name="vehicleId">The ID of the vehicle.</param>
		/// <param name="id">The ID of the repair.</param>
		/// <param name="repair">The updated repair information.</param>
		/// <returns>A redirect to the index view if successful, otherwise the edit view.</returns>
		[Authorize]
		[HttpPost("/admin/vehicle/{vehicleId}/repair/edit/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int vehicleId, int id, [Bind("Id,Name,Cost,VehicleId")] Repair repair)
		{
			if (id != repair.Id)
			{
				return NotFound();
			}

			ModelState.Remove("Vehicle");
			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(repair);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RepairExists(repair.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction("Index", new { vehicleId });
			}

			ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", repair.VehicleId);

			return View(repair);
		}

		/// <summary>
		/// Shows the delete repair confirmation form for a specific repair.
		/// </summary>
		/// <param name="vehicleId">The ID of the vehicle.</param>
		/// <param name="id">The ID of the repair.</param>
		/// <returns>A view with the delete repair confirmation form.</returns>
		[Authorize]
		[HttpGet("/admin/vehicle/{vehicleId}/repair/delete/{id}")]
		public async Task<IActionResult> Delete(int vehicleId, int id)
		{
			var repair = await _context.Repair
				.Include(r => r.Vehicle)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (repair == null)
			{
				return NotFound();
			}

			return View(repair);
		}

		/// <summary>
		/// Deletes a specific repair.
		/// </summary>
		/// <param name="vehicleId">The ID of the vehicle.</param>
		/// <param name="id">The ID of the repair.</param>
		/// <returns>A redirect to the index view.</returns>
		[Authorize]
		[HttpPost("/admin/vehicle/{vehicleId}/repair/delete/{id}"), ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int vehicleId, int id)
		{
			var repair = await _context.Repair.FindAsync(id);

			if (repair != null)
			{
				_context.Repair.Remove(repair);
			}

			await _context.SaveChangesAsync();

			return RedirectToAction("Index", new { vehicleId });
		}

		/// <summary>
		/// Checks if a repair exists.
		/// </summary>
		/// <param name="id">The ID of the repair to check.</param>
		/// <returns>True if the repair exists, otherwise false.</returns>
		private bool RepairExists(int id)
		{
			return _context.Repair.Any(e => e.Id == id);
		}
	}
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoituresV2.Data;
using ExpressVoituresV2.Models;
using ExpressVoituresV2.ViewModel;

namespace ExpressVoituresV2.Controllers
{
    public class RepairsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepairsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Repairs/5
        [HttpGet("/admin/vehicle/{vehicleId}/repair")]
        public async Task<IActionResult> Index(int vehicleId)
        {
            var vehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.Id == vehicleId);

            if (vehicle == null)
            {
                return NotFound();
            }

            var totalRepairCost = vehicle.Repairs?.Sum(r => r.Cost) ?? 0;

            var repairs = await _context.Repair
                .Where(r => r.VehicleId == vehicleId)
                .Select(r => new RepairViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Cost = r.Cost,
                    TotalRepairCost = totalRepairCost,
                    VehicleId = vehicle.Id,
                    Vehicle = vehicle,
                })
                .ToListAsync();

            if (!repairs.Any())
            {
                repairs = new List<RepairViewModel> { new RepairViewModel { VehicleId = vehicle.Id }};
            }

            return View(repairs);
        }

        // GET: Repairs/Create
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

        // POST: Repairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

		// GET: Repairs/Edit/5
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

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

		// GET: Repairs/Delete/5
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

        // POST: Repairs/Delete/5
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

        private bool RepairExists(int id)
        {
            return _context.Repair.Any(e => e.Id == id);
        }
    }
}

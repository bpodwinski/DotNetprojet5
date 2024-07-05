using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoituresV2.Data;
using ExpressVoituresV2.Models;
using System.Drawing.Drawing2D;

namespace ExpressVoituresV2
{
    public class TrimLevelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrimLevelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrimLevels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TrimLevels.Include(t => t.Model);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TrimLevels/Details/5
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

        // GET: TrimLevels/Create
        public IActionResult Create()
        {
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name");
            return PartialView("_CreatePartial");
        }

        // POST: TrimLevels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ModelId")] TrimLevel trimLevel)
        {
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

        // GET: TrimLevels/Edit/5
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

        // POST: TrimLevels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: TrimLevels/Delete/5
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

        // POST: TrimLevels/Delete/5
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

        private bool TrimLevelExists(int id)
        {
            return _context.TrimLevels.Any(e => e.Id == id);
        }
    }
}

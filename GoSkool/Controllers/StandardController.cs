using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoSkool.Data;
using GoSkool.Models;
using Microsoft.AspNetCore.Authorization;

namespace GoSkool.Controllers
{
    [Authorize(Roles ="Admin")]
    public class StandardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StandardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Standard
        public async Task<IActionResult> Index()
        {
            return View(await _context.Standard.ToListAsync());
        }

        // GET: Standard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var standardEntity = await _context.Standard
                .FirstOrDefaultAsync(m => m.Id == id);
            if (standardEntity == null)
            {
                return NotFound();
            }

            return View(standardEntity);
        }

        // GET: Standard/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Standard/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassNumber")] StandardEntity standardEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(standardEntity);
                var sections = await _context.Section.ToListAsync();
                foreach (var sec in sections)
                {
                    Console.WriteLine(sec.Name);
                    _context.Classes.Add(new ClassEntity() { Section=sec,Standard=standardEntity});
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(standardEntity);
        }

        // GET: Standard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var standardEntity = await _context.Standard.FindAsync(id);
            if (standardEntity == null)
            {
                return NotFound();
            }
            return View(standardEntity);
        }

        // POST: Standard/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassNumber")] StandardEntity standardEntity)
        {
            if (id != standardEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(standardEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StandardEntityExists(standardEntity.Id))
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
            return View(standardEntity);
        }

        // GET: Standard/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var standardEntity = await _context.Standard
                .FirstOrDefaultAsync(m => m.Id == id);
            if (standardEntity == null)
            {
                return NotFound();
            }

            return View(standardEntity);
        }

        // POST: Standard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var standardEntity = await _context.Standard.FindAsync(id);
            if (standardEntity != null)
            {
                _context.Standard.Remove(standardEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StandardEntityExists(int id)
        {
            return _context.Standard.Any(e => e.Id == id);
        }
    }
}

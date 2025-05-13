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
    [Authorize(Roles = "Admin")]
    public class SectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Section
        public async Task<IActionResult> Index()
        {
            return View(await _context.Section.ToListAsync());
        }

        // GET: Section/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sectionEntity = await _context.Section
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sectionEntity == null)
            {
                return NotFound();
            }

            return View(sectionEntity);
        }

        // GET: Section/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SectionEntity sectionEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sectionEntity);
                var standards = await _context.Standard.ToListAsync();
                foreach (var standard in standards)
                {
                    Console.WriteLine(standard.ClassNumber);
                    _context.Classes.Add(new ClassEntity() { Section = sectionEntity, Standard = standard });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sectionEntity);
        }

        // GET: Section/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sectionEntity = await _context.Section.FindAsync(id);
            if (sectionEntity == null)
            {
                return NotFound();
            }
            return View(sectionEntity);
        }

        // POST: Section/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SectionEntity sectionEntity)
        {
            if (id != sectionEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sectionEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectionEntityExists(sectionEntity.Id))
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
            return View(sectionEntity);
        }

        // GET: Section/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sectionEntity = await _context.Section
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sectionEntity == null)
            {
                return NotFound();
            }

            return View(sectionEntity);
        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sectionEntity = await _context.Section.FindAsync(id);
            if (sectionEntity != null)
            {
                _context.Section.Remove(sectionEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SectionEntityExists(int id)
        {
            return _context.Section.Any(e => e.Id == id);
        }
    }
}

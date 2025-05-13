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
using GoSkool.Views.Teacher;
using Microsoft.AspNetCore.Identity;

namespace GoSkool.Controllers
{
    
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TeacherController(ApplicationDbContext context,UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Teacher
        public async Task<IActionResult> Index()
        {
            var TeacherHomeObj = new TeacherHomeModel();
            GoSkoolUser curUser = (GoSkoolUser)_context.Users.Find((await _userManager.GetUserAsync(HttpContext.User)).Id);
            var teacherId = curUser.UserId;
            var teacher = _context.Teachers.Include(x => x.Classes).ThenInclude(x => x.Standard).Include(x => x.Classes).ThenInclude(x => x.Section).Where(x => x.Id == teacherId).SingleOrDefault();
            TeacherHomeObj.classes = teacher.Classes;
            return View(TeacherHomeObj);
        }

        // GET: Teacher/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherEntity = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacherEntity == null)
            {
                return NotFound();
            }

            return View(teacherEntity);
        }

        // GET: Teacher/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teacher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Subject,Contact")] TeacherEntity teacherEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacherEntity);
        }

        // GET: Teacher/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherEntity = await _context.Teachers.FindAsync(id);
            if (teacherEntity == null)
            {
                return NotFound();
            }
            return View(teacherEntity);
        }

        // POST: Teacher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Subject,Contact")] TeacherEntity teacherEntity)
        {
            if (id != teacherEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherEntityExists(teacherEntity.Id))
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
            return View(teacherEntity);
        }

        // GET: Teacher/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherEntity = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacherEntity == null)
            {
                return NotFound();
            }

            return View(teacherEntity);
        }

        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherEntity = await _context.Teachers.FindAsync(id);
            if (teacherEntity != null)
            {
                _context.Teachers.Remove(teacherEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherEntityExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}

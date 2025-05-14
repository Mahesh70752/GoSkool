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
using Microsoft.AspNetCore.Identity;
using GoSkool.Views.Student;

namespace GoSkool.Controllers
{
    [Authorize(Roles ="Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StudentController(ApplicationDbContext context,UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> GetCurrentStudentId()
        {
            GoSkoolUser curUser = (GoSkoolUser)_context.Users.Find((await _userManager.GetUserAsync(HttpContext.User)).Id);
            var studentId = curUser.UserId;
            return studentId;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            var studentId = await GetCurrentStudentId();
            var StudentHomePageObj = new StudentHomePageModel();
            StudentHomePageObj.Student = _context.Students.Include(student => student.Class).ThenInclude(cls=>cls.Section).Include(st=>st.Class).ThenInclude(cls=>cls.Standard).Where(student => student.Id == studentId).SingleOrDefault();
            StudentHomePageObj.Assignments = _context.Assignment.Include(x => x.Class).Include(x=>x.CompletedStudents).Where(x => x.Class.Id == StudentHomePageObj.Student.Class.Id).ToList();
            return View(StudentHomePageObj);

        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentEntity = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentEntity == null)
            {
                return NotFound();
            }

            return View(studentEntity);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Address")] StudentEntity studentEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentEntity);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentEntity = await _context.Students.FindAsync(id);
            if (studentEntity == null)
            {
                return NotFound();
            }
            return View(studentEntity);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Address")] StudentEntity studentEntity)
        {
            if (id != studentEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentEntityExists(studentEntity.Id))
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
            return View(studentEntity);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentEntity = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentEntity == null)
            {
                return NotFound();
            }

            return View(studentEntity);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentEntity = await _context.Students.FindAsync(id);
            if (studentEntity != null)
            {
                _context.Students.Remove(studentEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentEntityExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}

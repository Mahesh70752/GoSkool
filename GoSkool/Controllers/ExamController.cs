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
using GoSkool.Views.Exam;

namespace GoSkool.Controllers
{
    [Authorize(Roles ="Admin,Teacher")]
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exam
        public async Task<IActionResult> Index()
        {
            return View(await _context.Exam.ToListAsync());
        }

        // GET: Exam/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examEntity = await _context.Exam
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examEntity == null)
            {
                return NotFound();
            }

            return View(examEntity);
        }

        // GET: Exam/Create
        public IActionResult Create(int subjectId)
        {
            var examModelObj = new ExamModel();
            examModelObj.subjectId = subjectId;
            examModelObj.ExamDate = DateTime.Now;
            examModelObj.classList = _context.Classes.Include(cls => cls.Standard).Include(cls => cls.Section).Select(cls => new SelectListItem
            {
                Text = cls.Standard.ClassNumber.ToString() + cls.Section.Name,
                Value = cls.Id.ToString()
            });
            return View(examModelObj);
        }

        // POST: Exam/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamModel examModelObj)
        {
            if (ModelState.IsValid)
            {
                ExamEntity exam = new ExamEntity() { ClassId=Int32.Parse(examModelObj.ClassId),Name= examModelObj.Name,SubjectId=examModelObj.subjectId,ExamDate=examModelObj.ExamDate };
                var Students = _context.Students.Include(x=>x.Class).Where(x=>x.Class.Id==exam.ClassId).ToList();
                exam.studentMarks = new List<int>();
                foreach (var student in Students) {
                    exam.studentMarks.Add(0);
                }
                _context.Add(exam);
                await _context.SaveChangesAsync();
                foreach (var student in Students) {
                    if (student.Exams == null) student.Exams = new List<ExamEntity>();
                    student.Exams.Add(exam);
                    _context.Students.Update(student);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Teacher");
            }
            examModelObj.classList = _context.Classes.Include(cls => cls.Standard).Include(cls => cls.Section).Select(cls => new SelectListItem
            {
                Text = cls.Standard.ClassNumber.ToString() + cls.Section.Name,
                Value = cls.Id.ToString()
            });
            return View(examModelObj);
        }

        // GET: Exam/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examEntity = await _context.Exam.FindAsync(id);
            if (examEntity == null)
            {
                return NotFound();
            }
            return View(examEntity);
        }

        // POST: Exam/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SubjectId,ExamDate,ClassId")] ExamEntity examEntity)
        {
            if (id != examEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamEntityExists(examEntity.Id))
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
            return View(examEntity);
        }

        // GET: Exam/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examEntity = await _context.Exam
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examEntity == null)
            {
                return NotFound();
            }

            return View(examEntity);
        }

        // POST: Exam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examEntity = await _context.Exam.FindAsync(id);
            if (examEntity != null)
            {
                _context.Exam.Remove(examEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExamEntityExists(int id)
        {
            return _context.Exam.Any(e => e.Id == id);
        }
    }
}

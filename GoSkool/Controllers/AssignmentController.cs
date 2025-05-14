using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoSkool.Data;
using GoSkool.Models;
using GoSkool.Views.Assignment;

namespace GoSkool.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult TeacherAssignment(int asId)
        {
            Console.WriteLine(asId);
            var TeacherAssignmentObj = new TeacherAssignmentModel();
            TeacherAssignmentObj.Assignment = _context.Assignment.Include(assignment=>assignment.Class).ThenInclude(cls=>cls.Section).Include(a=>a.Class).ThenInclude(cls=>cls.Standard).Include(assignment=>assignment.CompletedStudents).Where(x=>asId==x.Id).SingleOrDefault();
            TeacherAssignmentObj.Students = new List<Tuple<StudentEntity, string>>();
            Console.WriteLine(TeacherAssignmentObj.Assignment.Title);
            var AllStudents = _context.Students.Include(student => student.Class).Where(student => student.Class.Id == TeacherAssignmentObj.Assignment.Class.Id).ToList();
            foreach (var student in AllStudents) {
                var Status = "Completed";
                if (TeacherAssignmentObj.Assignment.CompletedStudents == null || !TeacherAssignmentObj.Assignment.CompletedStudents.Contains(student))
                    Status = "Incompleted";
                TeacherAssignmentObj.Students.Add(new Tuple<StudentEntity, string>(student, Status));
            }
            return View(TeacherAssignmentObj);
        }

        // GET: Assignment
        public async Task<IActionResult> Index()
        {
            return View(await _context.Assignment.ToListAsync());
        }

        // GET: Assignment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentEntity = await _context.Assignment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignmentEntity == null)
            {
                return NotFound();
            }

            return View(assignmentEntity);
        }

        // GET: Assignment/Create
        public IActionResult Create(int teacherId)
        {
            var AssignmentCreationObj = new AssignmentCreationModel();
            var Teacher = _context.Teachers.Include(teacher => teacher.Classes).ThenInclude(Class => Class.Standard).Include(teacher => teacher.Classes).ThenInclude(Class => Class.Section).Where(teacher => teacher.Id == teacherId).SingleOrDefault();
            AssignmentCreationObj.ClassList = Teacher.Classes.Select(Class=>new SelectListItem
            {
                Text = Class.Standard.ClassNumber.ToString() + Class.Section.Name.ToString(),
                Value = Class.Id.ToString()
            });
            return View(AssignmentCreationObj);
        }

        // POST: Assignment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AssignmentCreationModel assignmentCreationObj)
        {
            assignmentCreationObj.Assignment.Class = _context.Classes.Find(Int32.Parse(assignmentCreationObj.classId));
            Console.WriteLine("We are inside valid state model");
            _context.Add(assignmentCreationObj.Assignment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Teacher");
        }

        // GET: Assignment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentEntity = await _context.Assignment.FindAsync(id);
            if (assignmentEntity == null)
            {
                return NotFound();
            }
            return View(assignmentEntity);
        }

        // POST: Assignment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] AssignmentEntity assignmentEntity)
        {
            if (id != assignmentEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignmentEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentEntityExists(assignmentEntity.Id))
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
            return View(assignmentEntity);
        }

        // GET: Assignment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentEntity = await _context.Assignment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignmentEntity == null)
            {
                return NotFound();
            }

            return View(assignmentEntity);
        }

        // POST: Assignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignmentEntity = await _context.Assignment.FindAsync(id);
            if (assignmentEntity != null)
            {
                _context.Assignment.Remove(assignmentEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentEntityExists(int id)
        {
            return _context.Assignment.Any(e => e.Id == id);
        }
    }
}

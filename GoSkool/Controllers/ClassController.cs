using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoSkool.Data;
using GoSkool.Models;
using GoSkool.Views.Class;
using NuGet.Versioning;
using Microsoft.AspNetCore.Authorization;

namespace GoSkool.Controllers
{
    public class ClassController : Controller
    {
        private readonly ApplicationDbContext _context;
        

        public IndexModel IndexObj {  get; set; }

        public ClassController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int classId)
        {
            IndexObj = new IndexModel();
            IndexObj.teachers = _context.Teachers.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            IndexObj.Class = _context.Classes.Include(s => s.Standard).Include(s => s.Section).Include(s => s.Subjects).ThenInclude(s=>s.Teacher).Where(s => s.Id == classId).SingleOrDefault();
            if (IndexObj.Class == null)
            {
                Console.WriteLine("class not loaded properly");
                return RedirectToAction("Index", "Admin");
            }
            IndexObj.Students = _context.Students.Include(x => x.Class).Where(x => x.Class.Id == IndexObj.Class.Id).ToList();
            return View(IndexObj);
        }

        public async Task<IActionResult> AssignTeacher(int classId,int subjectId)
        {
            Console.WriteLine("We are inside assgin teacher");
            var TeacherId = Request.Form["TeacherSelection"];
            var Teacher = _context.Teachers.Find(Int32.Parse(TeacherId));
            if(Teacher.Classes==null) Teacher.Classes = new List<ClassEntity>();
            Teacher.Classes.Add(_context.Classes.Find(classId));
            _context.Teachers.Update(Teacher);
            var subject = _context.Subject.Find(subjectId);
            subject.Teacher = Teacher;
            _context.Subject.Update(subject);
            await _context.SaveChangesAsync();
            Console.WriteLine(subject.Teacher.Name);
            return RedirectToAction("Index", new { classId = classId });
        }


        public IActionResult AddSubject(int classId)
        {
            AddSubject addSubject = new AddSubject() { ClassId = classId };
            return View(addSubject);
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateSubject(AddSubject addSubject)
        {
            var subject = new SubjectEntity() { Name = addSubject.Subject };
            subject.Teacher = null;
            _context.Subject.Add(subject);
            await _context.SaveChangesAsync();
            var classEntity = _context.Classes.Include(x => x.Subjects).Include(x => x.Standard).Include(x => x.Section).Where(s => s.Id == addSubject.ClassId).SingleOrDefault();
            if (classEntity.Subjects == null) classEntity.Subjects = new List<SubjectEntity>();
            classEntity.Subjects.Add(subject);
            _context.Classes.Update(classEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { classId = addSubject.ClassId });
        }
    }
}



using GoSkool.Data;
using GoSkool.Models;
using GoSkool.DTO;
using GoSkool.Views.Class;
using Microsoft.EntityFrameworkCore;

namespace GoSkool.Services
{
    public class ClassService: IClassService
    {
        private readonly ApplicationDbContext _context;

        public ClassService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool GetIndexObj(int classId,IndexModel IndexObj)
        {
            IndexObj.teachers = _context.Teachers.ToList();
            IndexObj.Class = _context.Classes.Include(s => s.Standard).Include(s => s.Section).Include(s => s.Subjects).ThenInclude(s => s.Teacher).Where(s => s.Id == classId).SingleOrDefault();
            if (IndexObj.Class == null)
            {
                return false;
            }
            IndexObj.Students = _context.Students.Include(x => x.Class).Where(x => x.Class.Id == IndexObj.Class.Id).ToList();
            return true;
        }

        public void AssignTeacher(int classId,int SubjectId,string TeacherId)
        {
            var Teacher = _context.Teachers.Find(Int32.Parse(TeacherId));
            if (Teacher.Classes == null) Teacher.Classes = new List<ClassEntity>();
            Teacher.Classes.Add(_context.Classes.Find(classId));
            _context.Teachers.Update(Teacher);
            var subject = _context.Subject.Find(SubjectId);
            subject.Teacher = Teacher;
            _context.Subject.Update(subject);
            _context.SaveChangesAsync();
        }

        public void CreateSubject(AddSubject addSubject)
        {
            var subject = new SubjectEntity() { Name = addSubject.Subject };
            subject.Teacher = null;
            _context.Subject.Add(subject);
            _context.SaveChangesAsync();
            var classEntity = _context.Classes.Include(x => x.Subjects).Include(x => x.Standard).Include(x => x.Section).Where(s => s.Id == addSubject.ClassId).SingleOrDefault();
            if (classEntity.Subjects == null) classEntity.Subjects = new List<SubjectEntity>();
            classEntity.Subjects.Add(subject);
            _context.Classes.Update(classEntity);
            _context.SaveChangesAsync();
        }
    }
}

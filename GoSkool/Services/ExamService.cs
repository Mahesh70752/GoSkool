using GoSkool.Data;
using GoSkool.Models;
using GoSkool.Views.Exam;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace GoSkool.Services
{
    public class ExamService : IExamService
    {
        private readonly ApplicationDbContext _context;

        public ExamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void GetExamModelObj(int subjectId, ExamModel examModelObj)
        {
            examModelObj.subjectId = subjectId;
            examModelObj.ExamDate = DateTime.Now;
            examModelObj.classList = _context.Classes.Include(cls => cls.Standard).Include(cls => cls.Section).Select(cls => new SelectListItem
            {
                Text = cls.Standard.ClassNumber.ToString() + cls.Section.Name,
                Value = cls.Id.ToString()
            });
        }

        public async Task<bool> CreateExam(ExamModel examModelObj)
        {
            ExamEntity exam = new ExamEntity() { ClassId = Int32.Parse(examModelObj.ClassId), Name = examModelObj.Name, SubjectId = examModelObj.subjectId, ExamDate = examModelObj.ExamDate };
            var Students = _context.Students.Include(x => x.Class).Where(x => x.Class.Id == exam.ClassId).ToList();
            exam.studentMarks = new List<int>();
            exam.students = new List<int>();
            foreach (var student in Students)
            {
                exam.studentMarks.Add(0);
                exam.students.Add(student.Id);
            }
            _context.Add(exam);
            await _context.SaveChangesAsync();
            foreach (var student in Students)
            {
                if (student.Exams == null) student.Exams = new List<ExamEntity>();
                student.Exams.Add(exam);
                _context.Students.Update(student);
            }
            await _context.SaveChangesAsync();
            examModelObj.classList = _context.Classes.Include(cls => cls.Standard).Include(cls => cls.Section).Select(cls => new SelectListItem
            {
                Text = cls.Standard.ClassNumber.ToString() + cls.Section.Name,
                Value = cls.Id.ToString()
            });
            return true;
            
            
        }

    }
}

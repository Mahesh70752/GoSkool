using GoSkool.Data;
using GoSkool.DTO;
using GoSkool.Models;
using GoSkool.Views.Teacher;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoSkool.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public TeacherService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void GetTeacherHomeObj(IdentityUser user,TeacherHomeModel TeacherHomeObj)
        {
            GoSkoolUser curUser = (GoSkoolUser)_context.Users.Find((user).Id);
            var teacherId = curUser.UserId;
            TeacherHomeObj.teacherId = teacherId;
            var teacher = _context.Teachers.Include(x => x.Classes).ThenInclude(x => x.Standard).Include(x => x.Classes).ThenInclude(x => x.Section).Where(x => x.Id == teacherId).SingleOrDefault();
            var subject = _context.Subject.Where(sub => sub.Name.Contains(teacher.Subject)).FirstOrDefault();
            if (subject != null)
            {
                TeacherHomeObj.subjectId = subject.Id;
            }
            TeacherHomeObj.classes = teacher.Classes;
            TeacherHomeObj.assignments = new List<AssignmentEntity>();
            TeacherHomeObj.Exams = new List<ExamEntity>();
            var AllExams = _context.Exam.ToList();
            foreach (var exam in AllExams)
            {
                var Class = _context.Classes.Find(exam.ClassId);
                if (Class == null) continue;
                if (TeacherHomeObj.classes.Contains(Class))
                    TeacherHomeObj.Exams.Add(exam);
            }
            foreach (var Class in teacher.Classes)
            {
                TeacherHomeObj.assignments.AddRange(_context.Assignment.Include(assignment => assignment.Class).Where(assignment => assignment.Class.Id == Class.Id).ToList());
            }
        }

        public int GetCurrentTeacherId(IdentityUser user)
        {
            GoSkoolUser curUser = (GoSkoolUser)_context.Users.Find((user).Id);
            var teacherId = curUser.UserId;
            return teacherId;
        }

        public void GetScheduleData(int teacherId, ScheduleModel teacherScheduleObj)
        {
            teacherScheduleObj.Schedule = _context.TeacherSchedule.Include(x => x.Teacher).Where(x => x.Teacher.Id == teacherId).Include(x => x.Class).ThenInclude(x => x.Standard).Include(x => x.Class).ThenInclude(x => x.Section).SingleOrDefault();
        }

        public void GetTeacherAssignments(int teacherId, TeacherHomeModel TeacherHomeObj)
        {
            TeacherHomeObj.teacherId = teacherId;
            var teacher = _context.Teachers.Include(x => x.Classes).ThenInclude(x => x.Standard).Include(x => x.Classes).ThenInclude(x => x.Section).Where(x => x.Id == teacherId).SingleOrDefault();
            TeacherHomeObj.classes = teacher.Classes;
            TeacherHomeObj.assignments = new List<AssignmentEntity>();
            foreach (var Class in teacher.Classes)
            {
                TeacherHomeObj.assignments.AddRange(_context.Assignment.Include(assignment => assignment.Class).Where(assignment => assignment.Class.Id == Class.Id).ToList());
            }
        }

        public void FillExamDetails(int ExamId, CheckExamDTO checkExamdto)
        {
            var exam = _context.Exam.Find(ExamId);
            var Class = _context.Classes.Include(cls=>cls.Standard).Include(cls=>cls.Section).Where(cls=>cls.Id==exam.ClassId).SingleOrDefault(); 
            var subject = _context.Subject.Find(exam.SubjectId);
            checkExamdto.ClassName = Class.Standard.ClassNumber.ToString()+Class.Section.Name;
            checkExamdto.ExamId = exam.Id;
            checkExamdto.ExamName = exam.Name;
            checkExamdto.ExamDate = exam.ExamDate;
            checkExamdto.StudentMarks = new List<Tuple<string, string,int>>();
            int n = exam.studentMarks.Count();
            if (exam.isCompleted)
            {
                checkExamdto.status = "Completed";
            }
            else
            {
                checkExamdto.status = "Not Completed";
            }
                for (int i = 0; i < n; i++)
                {
                    var student = _context.Students.Find(exam.students[i]);
                    var marks = "Not Given Yet";
                    if (exam.isCompleted)
                    {
                        marks = exam.studentMarks[i].ToString();
                    }
                    checkExamdto.StudentMarks.Add(new Tuple<string, string,int>(student.FirstName + ", " + student.LastName, marks,student.Id));
                }
        }

        public void AddStudentScore(CheckExamDTO checkExamdto)
        {
            
            var exam = _context.Exam.Find(checkExamdto.ExamId);
            for(int i = 0; i < exam.students.Count; i++)
            {
                if (exam.students[i] == checkExamdto.StudentId)
                {
                    exam.studentMarks[i] = checkExamdto.Score;
                    break;
                }
            }
            _context.Exam.Update(exam);
            _context.SaveChangesAsync().Wait();
        }
    }
}

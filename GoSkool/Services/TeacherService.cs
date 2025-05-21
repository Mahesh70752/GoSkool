using GoSkool.Data;
using GoSkool.Models;
using GoSkool.Views.Teacher;
using Microsoft.AspNetCore.Identity;
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
    }
}

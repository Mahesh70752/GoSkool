using GoSkool.Data;
using GoSkool.Models;
using GoSkool.Views.Student;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GoSkool.Services
{
    public class StudentService : IStudentService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public StudentService(UserManager<IdentityUser> userManager, ApplicationDbContext context) {
            _userManager = userManager;
            _context = context;
        }

        public int GetCurrentStudentId(IdentityUser user)
        {
            GoSkoolUser curUser = (GoSkoolUser)_context.Users.Find((user).Id);
            var studentId = curUser.UserId;
            return studentId;
        }

        public void GetStudentHomePageObj(IdentityUser user, StudentHomePageModel StudentHomePageObj)
        {
            var studentId = GetCurrentStudentId(user);
            StudentHomePageObj.Student = _context.Students.Include(student => student.Class).ThenInclude(cls => cls.Section).Include(st => st.Class).ThenInclude(cls => cls.Standard).Where(student => student.Id == studentId).SingleOrDefault();
            StudentHomePageObj.Assignments = _context.Assignment.Include(x => x.Class).Include(x => x.CompletedStudents).Where(x => x.Class.Id == StudentHomePageObj.Student.Class.Id).ToList();

        }

        public void GetClassScheduleObj(IdentityUser user, ClassScheduleModel scheduleObj)
        {
            var studentId = GetCurrentStudentId(user);
            var Student = _context.Students.Include(x => x.Class).Where(x => x.Id == studentId).SingleOrDefault();
            scheduleObj.schedule = _context.classSchedule.Include(x => x.periods).Include(x => x.Class).Where(x => x.Class.Id == Student.Class.Id).SingleOrDefault();
        }
    }
}

using GoSkool.Data;
using GoSkool.DTO;
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

        public void GetScheduleData(int studentId, ClassScheduleDTO classScheduleDTO)
        {
            var student = _context.Students.Include(x=>x.Class).ThenInclude(x=>x.Standard).Include(x=>x.Class).ThenInclude(x=>x.Section).Where(x=>x.Id==studentId).SingleOrDefault();
            classScheduleDTO.Class = student.Class;
            classScheduleDTO.ClassSchedule = _context.ClassSchedule.Include(x => x.ClassScheduleTeachers).ThenInclude(x => x.Teacher).Include(x => x.Class).Where(x => x.Class.Id == student.Class.Id).SingleOrDefault();
        }

        public void GetStudentHomePageObj(IdentityUser user, StudentHomePageModel StudentHomePageObj)
        {
            var studentId = GetCurrentStudentId(user);
            StudentHomePageObj.Student = _context.Students.Include(student => student.Class).ThenInclude(cls => cls.Section).Include(st => st.Class).ThenInclude(cls => cls.Standard).Where(student => student.Id == studentId).SingleOrDefault();
            StudentHomePageObj.Exams = _context.Exam.Where(x => x.ClassId == StudentHomePageObj.Student.Class.Id).ToList();
            StudentHomePageObj.Subjects = new List<string>();
            StudentHomePageObj.Marks = new List<int>();
            foreach(var exam in StudentHomePageObj.Exams)
            {
                var sub = _context.Subject.Find(exam.SubjectId);
                StudentHomePageObj.Subjects.Add(sub.Name);
                for(int i=0;i<exam.students.Count;i++)
                {
                    if (exam.students[i] == studentId)
                    {
                        StudentHomePageObj.Marks.Add(exam.studentMarks[i]);
                        break;
                    }
                }
            }
            StudentHomePageObj.Assignments = _context.Assignment.Include(x => x.Class).Include(x => x.CompletedStudents).Where(x => x.Class.Id == StudentHomePageObj.Student.Class.Id).ToList();

        }

        public void GetClassScheduleObj(IdentityUser user, ClassScheduleModel scheduleObj)
        {
            var studentId = GetCurrentStudentId(user);
            var Student = _context.Students.Include(x => x.Class).Where(x => x.Id == studentId).SingleOrDefault();
            //scheduleObj.schedule = _context.classSchedule.Include(x => x.periods).Include(x => x.Class).Where(x => x.Class.Id == Student.Class.Id).SingleOrDefault();
        }

        public void UploadAssignment(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            if (extension != ".pdf")
            {
                Console.WriteLine("Please upload only PDF files");
                return;
            }
            string fileName = Guid.NewGuid().ToString() + extension;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            path = Path.Combine(path, fileName);
            using FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);

        }
    }
}

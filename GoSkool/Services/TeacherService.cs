using GoSkool.Data;
using GoSkool.DTO;
using GoSkool.Models;
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

        public void GetScheduleData(int teacherId, TeacherScheduleDTO teacherScheduleDTO)
        {
            teacherScheduleDTO.Teacher = _context.Teachers.Find(teacherId);
            teacherScheduleDTO.TeacherSchedule = _context.TeachersSchedule.Include(x => x.Teacher).Include(x => x.TeacherScheduleClasses).ThenInclude(x => x.Class).ThenInclude(x=>x.Standard).Include(x=>x.TeacherScheduleClasses).ThenInclude(x=>x.Class).ThenInclude(x=>x.Section).Where(x => x.Teacher.Id == teacherId).SingleOrDefault();
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

        public void FillClassDetails(TeacherClassDTO classDTO,int teacherId, int ClassId)
        {
            classDTO.teacher = _context.Teachers.Find(teacherId);
            var exams = _context.Exam.Where(ex=>(ex.ClassId == ClassId)).ToList();
            classDTO.Exams = new List<ExamEntity>();
            foreach(var exam in exams)
            {
                var subject = _context.Subject.Find(exam.SubjectId);
                if(subject.Name.Contains(classDTO.teacher.Subject)) classDTO.Exams.Add(exam);
            }
            classDTO.Assignments = _context.Assignment.Include(asg=>asg.Class).Where(asg=>asg.Class.Id== ClassId).ToList();
        }

        public void FillAttendanceRecords(int teacherId, TakeAttendanceDTO takeAttendanceDTO)
        {


            int PeriodNumber = 0;
            TimeOnly t = new TimeOnly(9, 0);
            var curTime = TimeOnly.FromDateTime(DateTime.Now);
            while (true)
            {
                PeriodNumber++;
                if (PeriodNumber == 3 || PeriodNumber == 7) t = t.AddMinutes(10);
                if (PeriodNumber == 5) t = t.AddMinutes(40);
                var et = t.AddMinutes(60);
                if (curTime < et && curTime >= t)
                {
                    break;
                }
                if (PeriodNumber == 9)
                {
                    takeAttendanceDTO.Break = true;
                    return;
                }
                t = et;
            }
            int ClassId = _context.TeachersSchedule.Where(x => x.Teacher.Id == teacherId).Include(x => x.Teacher).Include(x => x.TeacherScheduleClasses).ThenInclude(x=>x.Class).SingleOrDefault().TeacherScheduleClasses[PeriodNumber - 1].Class.Id;
            if (ClassId == 82)
            {
                takeAttendanceDTO.Break = true;
                return;
            }
            takeAttendanceDTO.Class = _context.Classes.Include(x => x.Standard).Include(x => x.Section).Where(x => x.Id == ClassId).SingleOrDefault();
            takeAttendanceDTO.PeriodNumber = PeriodNumber;
            var AttList = _context.Attendance.Include(x=>x.Class).Include(x=>x.Student).Where(x=>(x.Class.Id==ClassId&&x.PeriodNumber==PeriodNumber&&x.Date==DateOnly.FromDateTime(DateTime.Now))).ToList();
            if (AttList != null&&AttList.Count!=0)
            {
                takeAttendanceDTO.AttendanceRecords = AttList;
                takeAttendanceDTO.AttendanceTaken = true;
                return;
            }
            
            takeAttendanceDTO.AttendanceRecords = new List<AttendanceEntity>();
            var ClassStudents = _context.Students.Include(x=>x.Class).Where(x=>x.Class.Id== ClassId).ToList();
            foreach(var student in ClassStudents)
            {
                takeAttendanceDTO.AttendanceRecords.Add(new AttendanceEntity() { PeriodNumber=PeriodNumber,Class=takeAttendanceDTO.Class, Date =  DateOnly.FromDateTime(DateTime.Now) , Student = student});
            }
        }

        public void SubmitAttendance(TakeAttendanceDTO takeAttendanceDTO)
        {
            var Class = _context.Classes.Find(takeAttendanceDTO.ClassId);
            for(int i=0;i<takeAttendanceDTO.Students.Count;i++)
            {
                var curStudent = _context.Students.Find(takeAttendanceDTO.Students[i]);
                var curStudentAttendanceRecord = new AttendanceEntity() { Class = Class, Student = curStudent, Date = DateOnly.FromDateTime(DateTime.Now), Present = takeAttendanceDTO.AttendanceRecords[i].Present,PeriodNumber=takeAttendanceDTO.PeriodNumber };
                _context.Attendance.Add(curStudentAttendanceRecord);
            }
            _context.SaveChanges();
        }
    }
}

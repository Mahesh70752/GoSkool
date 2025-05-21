using GoSkool.Data;
using GoSkool.Models;
using GoSkool.Views.Assignment;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GoSkool.Services
{
    public class AssignmentService : IAssignmentService
    {
        public readonly ApplicationDbContext _context;
        public AssignmentService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void GetTeacherAssignment(int asId,TeacherAssignmentModel TeacherAssignmentObj)
        {
            TeacherAssignmentObj.Assignment = _context.Assignment.Include(assignment => assignment.Class).ThenInclude(cls => cls.Section).Include(a => a.Class).ThenInclude(cls => cls.Standard).Include(assignment => assignment.CompletedStudents).Where(x => asId == x.Id).SingleOrDefault();
            TeacherAssignmentObj.Students = new List<Tuple<StudentEntity, string>>();
            var AllStudents = _context.Students.Include(student => student.Class).Where(student => student.Class.Id == TeacherAssignmentObj.Assignment.Class.Id).ToList();
            foreach (var student in AllStudents)
            {
                var Status = "Completed";
                if (TeacherAssignmentObj.Assignment.CompletedStudents == null || !TeacherAssignmentObj.Assignment.CompletedStudents.Contains(student))
                    Status = "Incompleted";
                TeacherAssignmentObj.Students.Add(new Tuple<StudentEntity, string>(student, Status));
            }
        }

        public async Task<List<AssignmentEntity>> GetAssignmentsAsync()
        {
            return await _context.Assignment.ToListAsync();
        }

        public void GetAssignmentCreationObj(int teacherId, AssignmentCreationModel AssignmentCreationObj)
        {
            var Teacher = _context.Teachers.Include(teacher => teacher.Classes).ThenInclude(Class => Class.Standard).Include(teacher => teacher.Classes).ThenInclude(Class => Class.Section).Where(teacher => teacher.Id == teacherId).SingleOrDefault();
            AssignmentCreationObj.ClassList = Teacher.Classes.Select(Class => new SelectListItem
            {
                Text = Class.Standard.ClassNumber.ToString() + Class.Section.Name.ToString(),
                Value = Class.Id.ToString()
            });
        }

        public void CreateAssignment(AssignmentCreationModel assignmentCreationObj)
        {
            assignmentCreationObj.Assignment.Class = _context.Classes.Find(Int32.Parse(assignmentCreationObj.classId));
            _context.Add(assignmentCreationObj.Assignment);
            _context.SaveChangesAsync();
        }

    }
}

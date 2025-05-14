using GoSkool.Models;

namespace GoSkool.Views.Teacher
{
    public class TeacherHomeModel
    {
        public IEnumerable<ClassEntity> classes { get; set; }
        public IEnumerable<AssignmentEntity> assignments { get; set; }
    }
}

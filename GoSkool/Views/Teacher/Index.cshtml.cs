using GoSkool.Models;

namespace GoSkool.Views.Teacher
{
    public class TeacherHomeModel
    {
        public IEnumerable<ClassEntity> classes { get; set; }
        public List<AssignmentEntity> assignments { get; set; }

        public List<ExamEntity> Exams { get; set; }
        public int subjectId { get; set; }

        public int teacherId { get; set; }
    }
}

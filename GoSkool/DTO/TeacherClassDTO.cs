
using GoSkool.Models;

namespace GoSkool.DTO
{
    public class TeacherClassDTO
    {
        public List<ExamEntity> Exams { get; set; }
        public List<AssignmentEntity> Assignments{ get; set; }
        public TeacherEntity teacher {  get; set; }

    }
}

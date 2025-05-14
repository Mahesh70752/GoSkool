using GoSkool.Models;

namespace GoSkool.Views.Student
{
    public class StudentHomePageModel
    {
        public StudentEntity Student { get; set; }
        public List<AssignmentEntity> Assignments { get; set; }
    }
}

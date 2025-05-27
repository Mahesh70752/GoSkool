using GoSkool.Models;

namespace GoSkool.DTO
{
    public class StudentHomePageModel
    {
        public StudentEntity Student { get; set; }
        public List<AssignmentEntity> Assignments { get; set; }
        public List<ExamEntity> Exams { get; set; }
        public List<string> Subjects { get; set; }
        public IFormFile file { get; set; }
        public List<int> Marks { get; set; }
    }
}

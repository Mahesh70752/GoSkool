using GoSkool.Models;

namespace GoSkool.DTO
{
    public class CheckExamDTO
    {
        public string ClassName { get; set; }
        public Dictionary<string, string> StudentMarks { get; set; }
        public string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
    }
}

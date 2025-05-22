using GoSkool.Models;

namespace GoSkool.DTO
{
    public class CheckExamDTO
    {
        public string ClassName { get; set; }
        public List<Tuple<string, string, int>> StudentMarks { get; set; }
        public string ExamName { get; set; }
        public int ExamId {  get; set; }
        public int StudentId { get; set; }
        public DateTime ExamDate { get; set; }
        public string status { get; set; }
        public string SubjectName {get; set; }
        public int Score {  get; set; }
    }
}

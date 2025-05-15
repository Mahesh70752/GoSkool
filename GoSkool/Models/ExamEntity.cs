namespace GoSkool.Models
{
    public class ExamEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubjectId { get; set; }
        public DateTime ExamDate { get; set; }
        public int ClassId{  get; set; }
        public List<int> studentMarks { get; set; }
        public bool isCompleted {  get; set; } =false;
    }
}

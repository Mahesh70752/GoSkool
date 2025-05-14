namespace GoSkool.Models
{
    public class AssignmentEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ClassEntity Class { get; set; }

        public IEnumerable<StudentEntity>? CompletedStudents { get; set; }
    }
}

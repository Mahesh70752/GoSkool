namespace GoSkool.Models
{
    public class StudentEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ClassEntity Class { get; set; }
        public string Address { get; set; }
        public IEnumerable<AssignmentEntity>? Assignments { get; set; }
    }
}

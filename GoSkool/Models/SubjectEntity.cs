namespace GoSkool.Models
{
    public class SubjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TeacherEntity? Teacher { get; set; }
    }
}

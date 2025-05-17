namespace GoSkool.Models
{
    public class ClassScheduleEntity
    {
        public int Id { get; set; }
        public ClassEntity Class { get; set; }
        public List<TeacherEntity> periods { get; set; } = new List<TeacherEntity>(9);
    }
}

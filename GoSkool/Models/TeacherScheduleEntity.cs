namespace GoSkool.Models
{
    public class TeacherScheduleEntity
    {
        public int Id { get; set; }
        public TeacherEntity Teacher {  get; set; }
        public List<ClassEntity> Class { get; set; } = new List<ClassEntity>(9);
    }
}

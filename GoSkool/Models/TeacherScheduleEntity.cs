namespace GoSkool.Models
{
    public class TeacherScheduleEntity
    {
        public int Id { get; set; }
        public TeacherEntity Teacher { get; set; }
        public List<TeacherScheduleClassEntity> TeacherScheduleClasses { get; set; }
    }
}

namespace GoSkool.Models
{
    public class ClassScheduleTeacherEntity
    {
        public int Id { get; set; }
        public int ClassScheduleId { get; set; }
        public ClassScheduleEntity ClassSchedule { get; set; }
        public int TeacherId { get; set; }
        public TeacherEntity Teacher { get; set; }
    }
}

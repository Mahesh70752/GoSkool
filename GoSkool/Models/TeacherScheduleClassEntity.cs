namespace GoSkool.Models
{
    public class TeacherScheduleClassEntity
    {
        public int Id { get; set; }
        public TeacherScheduleEntity TeacherSchedule { get; set; }
        public int TeacherScheduleId { get; set; }
        public ClassEntity Class {  get; set; }
        public int ClassId { get; set; }
    }
}

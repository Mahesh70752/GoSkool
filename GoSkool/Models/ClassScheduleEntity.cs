namespace GoSkool.Models
{
    public class ClassScheduleEntity
    {
        public int Id { get; set; }
        public ClassEntity Class { get; set; }
        public List<ClassScheduleTeacherEntity> ClassScheduleTeachers { get; set; }
    }
}

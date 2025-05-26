namespace GoSkool.Models
{
    public class TeacherEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject {  get; set; }
        public string Contact {  get; set; }
        public ICollection<ClassEntity> Classes { get; set; }

        public ICollection<TeacherPeriodEntity> Periods { get; set; }
    }
}

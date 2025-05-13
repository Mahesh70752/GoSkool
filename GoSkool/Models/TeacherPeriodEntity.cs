namespace GoSkool.Models
{
    public class TeacherPeriodEntity
    {
        public int Id { get; set; }
        public ClassEntity Class { get; set; }
        public bool breakPeriod {  get; set; }
    }
}

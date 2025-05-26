using System.Security.Permissions;

namespace GoSkool.Models
{
    public class AttendanceEntity
    {
        public int Id { get; set; }
        public ClassEntity Class { get; set; }
        public StudentEntity Student { get; set; }
        public DateOnly Date {  get; set; }
        public Boolean Present { get; set; }
        public int PeriodNumber { get; set; }
    }
}

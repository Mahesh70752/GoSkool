using GoSkool.Models;

namespace GoSkool.DTO
{
    public class TakeAttendanceDTO
    {
        public ClassEntity Class { get; set; }
        public int ClassId {  get; set; }
        public int PeriodNumber {get; set; }
        public List<AttendanceEntity> AttendanceRecords { get; set; }
        public List<int> Students { get; set; }
        public Boolean Break {  get; set; }
        public Boolean AttendanceTaken {  get; set; }
    }
}

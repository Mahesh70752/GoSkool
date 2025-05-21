using GoSkool.Models;

namespace GoSkool.DTO
{
    public class TimeTableViewModel
    {
        public TimeTableModel timeTableModelObj { get; set; }
        public List<TeacherEntity> Teachers { get; set; }
        public ClassEntity Class {  get; set; }
    }
}

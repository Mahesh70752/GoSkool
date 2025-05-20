using GoSkool.Models;

namespace GoSkool.Views.Admin
{
    public class TimeTableViewModel
    {
        public TimeTableModel timeTableModelObj { get; set; }
        public List<TeacherEntity> Teachers { get; set; }
        public ClassEntity Class {  get; set; }
    }
}

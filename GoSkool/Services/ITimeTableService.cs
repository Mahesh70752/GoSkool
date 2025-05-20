using GoSkool.Views.Admin;

namespace GoSkool.Services
{
    public interface ITimeTableService
    {
        public bool CheckTimeTableData(TimeTableModel timeTableModelObj);
        public TimeTableViewModel CreateTimeTable(TimeTableModel timeTableModelObj);
    }
}

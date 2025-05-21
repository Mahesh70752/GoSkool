using GoSkool.DTO;

namespace GoSkool.Services
{
    public interface ITimeTableService
    {
        public bool CheckTimeTableData(TimeTableModel timeTableModelObj);
        public TimeTableViewModel CreateTimeTable(TimeTableModel timeTableModelObj);
    }
}

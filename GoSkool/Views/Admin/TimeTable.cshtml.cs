using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoSkool.Views.Admin
{
    public class TimeTableModel
    {
        public string name {  get; set; } = string.Empty;
        public string ClassId {  get; set; }
        public IEnumerable<SelectListItem>? ClassList { get; set; }
        [DataType(DataType.Time)]
        public DateTime StartTime {  get; set; }
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
        public int NumberOfPeriods { get; set; }
        public bool MorningBreakRequired { get; set; }
        public bool EveningBreakRequired { get; set; }
        public int PeriodDuration { get; set; }
        public int LunchBreakDuration { get; set; }
        public int LunchBreakBeforePeriods { get; set; }
        public int? MorningBreakDuration {  get; set; }
        public int? MorningBreakBeforePeriods { get; set; }
        public int? EveningBreakDuration { get; set; }
        public int? EveningBreakBeforePeriods { get; set; }

        public string EndTimeError { get; set; } = string.Empty;
        public string NumberOfPeriodsError { get; set; } = string.Empty;
        public string PeriodDurationError { get; set; } = string.Empty;
        public string LunchBreakDurationError { get;set; } = string.Empty;
        public string EveningBreakDurationError { get;set;} = string.Empty;
        public string MorningBreakDurationError { get; set; } = string.Empty; 
        public string MorningBreakBeforePeriodsError { get; set; } = string.Empty;
        public string LunchBreakBeforePeriodsError { get; set; } = string.Empty;
        public string EveningBreakBeforePeriodsError { get; set; } = string.Empty;

        public string TimeError {  get; set; } = string.Empty;


    }
}

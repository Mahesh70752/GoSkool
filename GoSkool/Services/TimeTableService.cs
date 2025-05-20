using GoSkool.Data;
using GoSkool.Models;
using GoSkool.Views.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GoSkool.Services
{
    public class TimeTableService : ITimeTableService
    {
        private readonly ApplicationDbContext _context;

        public TimeTableService(ApplicationDbContext context)
        {
            _context = context;
        }
        bool ITimeTableService.CheckTimeTableData(TimeTableModel timeTableModelObj)
        {
            timeTableModelObj.ClassList = _context.Classes.Include(x => x.Standard).Include(x => x.Section).Select(x => new SelectListItem
            {
                Text = x.Standard.ClassNumber.ToString() + x.Section.Name,
                Value = x.Id.ToString()
            });
            if (timeTableModelObj.name == string.Empty)
            {
                
                return false;
            }
            if (timeTableModelObj.EndTime <= timeTableModelObj.StartTime)
            {
                timeTableModelObj.EndTimeError = "End time should be greater than start time";
                return false;
            }
            if (timeTableModelObj.NumberOfPeriods <= 2 || timeTableModelObj.NumberOfPeriods > 9)
            {
                timeTableModelObj.NumberOfPeriodsError = "Periods should be between 3 and 9";
                return false;
            }
            if (timeTableModelObj.PeriodDuration < 30 || timeTableModelObj.PeriodDuration > 60)
            {
                timeTableModelObj.PeriodDurationError = "period duration must be between 30 mins to 60mins";
                return false;
            }
            if (timeTableModelObj.LunchBreakBeforePeriods <= 0 || timeTableModelObj.LunchBreakBeforePeriods >= timeTableModelObj.NumberOfPeriods)
            {
                timeTableModelObj.LunchBreakBeforePeriodsError = "Lunch Break must be after 1st period and before last period";
                return false;
            }
            if (timeTableModelObj.MorningBreakBeforePeriods >= timeTableModelObj.LunchBreakBeforePeriods || timeTableModelObj.MorningBreakBeforePeriods <= 0)
            {
                timeTableModelObj.MorningBreakBeforePeriodsError = "Morning break must be between start time and Lunch Break";
                return false;
            }
            if (timeTableModelObj.EveningBreakBeforePeriods <= timeTableModelObj.LunchBreakBeforePeriods || timeTableModelObj.EveningBreakBeforePeriods >= timeTableModelObj.NumberOfPeriods)
            {
                timeTableModelObj.EveningBreakBeforePeriodsError = "Evening break must be between Lunch Break and end time";
                return false;
            }
            TimeSpan ts = timeTableModelObj.EndTime - timeTableModelObj.StartTime;
            double totalTime = ts.TotalMinutes;
            double timeTaken = timeTableModelObj.PeriodDuration * timeTableModelObj.NumberOfPeriods + timeTableModelObj.LunchBreakDuration;
            if (timeTableModelObj.MorningBreakRequired) timeTaken += (double)timeTableModelObj.MorningBreakDuration;
            if (timeTableModelObj.EveningBreakRequired) timeTaken += (double)timeTableModelObj.EveningBreakDuration;
            if (totalTime < timeTaken)
            {
                timeTableModelObj.TimeError = "Time is not sufficient. Either increase time between start and end times. Or decrease periods/durations";
                return false;
            }
            if(totalTime > timeTaken)
            {
                timeTableModelObj.TimeError = "Timings doesn't match, Periods are too short. Increase period duration or number of periods.";
                return false;
            }

            return true;
        }

        public TimeTableViewModel CreateTimeTable(TimeTableModel timeTableModelObj)
        {
            var ClassId = Int32.Parse(timeTableModelObj.ClassId);
            var Class = _context.Classes.Include(x => x.Section).Include(x => x.Standard).Include(x => x.Subjects).ThenInclude(x => x.Teacher).Where(x => x.Id == ClassId).FirstOrDefault();
            var ClassScheduleData = new List<int>();
            var TeacherScheduleData = new Dictionary<TeacherEntity, List<ClassEntity>>();
            var periods = timeTableModelObj.NumberOfPeriods;
            for (int i = 0; i < periods; i++)
            {
                ClassScheduleData.Add(-1);
            }
            foreach(var sub in Class.Subjects)
            {
                TeacherScheduleData[sub.Teacher] = new List<ClassEntity>();
            }
            Console.WriteLine("Filling time table");
            if (Fill(Class, 0, ClassScheduleData, TeacherScheduleData))
            {
                Console.Write("Time table created");
                var timetable = new TimeTableViewModel();
                timetable.Teachers = new List<TeacherEntity>();
                foreach (var i in ClassScheduleData)
                {
                    timetable.Teachers.Add(_context.Teachers.Find(i));
                }
                timetable.timeTableModelObj = timeTableModelObj;
                timetable.Class = Class;
                return timetable;
            }
            else
            {
                Console.Write("Failed to Create timetable");
                return null;
            }
        }

        public bool Fill(ClassEntity Class, int j, List<int> ClassScheduleData, Dictionary<TeacherEntity, List<ClassEntity>> TeacherScheduleData)
        {
            if (j == ClassScheduleData.Count) return true;
            foreach (var sub in Class.Subjects)
            {
                if (TeacherScheduleData[sub.Teacher].Contains(Class)) continue;
                ClassScheduleData[j] = sub.Teacher.Id;
                TeacherScheduleData[sub.Teacher].Add(Class);
                return Fill(Class, j + 1, ClassScheduleData,TeacherScheduleData);
            }
            return true;
        }
    }
}

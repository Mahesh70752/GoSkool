using GoSkool.Data;
using GoSkool.Models;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace GoSkool.BackGroundTasks
{
    [DisallowConcurrentExecution]
    public class TimeTableCreationJob : IJob
    {
        private readonly ILogger<TimeTableCreationJob> _logger;
        private readonly ApplicationDbContext _context;

        public TimeTableCreationJob(ILogger<TimeTableCreationJob> logger,ApplicationDbContext context) {
            _logger = logger;
            _context = context;
        }

        public bool FillTimeTable(int i,int j,List<ClassEntity> AllClasses, List<List<int>> ClassScheduleData, Dictionary<int, List<int>> TeacherClassData, Dictionary<int, List<int>> TeacherScheduleData)
        {
            if (j == 9)
            {
                return FillTimeTable(i+1,0,AllClasses,ClassScheduleData,TeacherClassData,TeacherScheduleData);
            }
            if (i == AllClasses.Count)
            {
                return true;
            }
            var ClassId = AllClasses[i].Id;
            foreach (var subject in AllClasses[i].Subjects)
            {
                var teacherId = subject.Teacher.Id;
                if (TeacherScheduleData[teacherId][j]==-1 && (!TeacherClassData[teacherId].Contains(ClassId)))
                {
                    TeacherClassData[teacherId].Add(ClassId);
                    ClassScheduleData[i][j] = teacherId;
                    TeacherScheduleData[teacherId][j] = ClassId;
                    if (FillTimeTable(i, j + 1, AllClasses, ClassScheduleData, TeacherClassData, TeacherScheduleData)) return true;
                    TeacherClassData[teacherId].Remove(ClassId);
                    ClassScheduleData[i][j] = -1;
                    TeacherScheduleData[teacherId][j] = -1;
                }
            }
            return false;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var AllClasses = _context.Classes.Include(cls=>cls.Subjects).ThenInclude(sub=>sub.Teacher).Include(cls=>cls.Standard).Include(cls=>cls.Section).ToList();
            var AllTeachers = _context.Teachers.ToList();
            List<List<int>> ClassScheduleData = new List<List<int>>();
            Dictionary<int, List<int>> TeacherClassData = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> TeacherScheduleData = new Dictionary<int, List<int>>();
            foreach (var Class in AllClasses)
            {
                ClassScheduleData.Add(new List<int>(9));
            }
            foreach(var teacher in AllTeachers)
            {
                TeacherClassData.Add(teacher.Id, new List<int>());
                TeacherScheduleData.Add(teacher.Id , new List<int>(9));
            }
            if (FillTimeTable(0,0,AllClasses,ClassScheduleData,TeacherClassData,TeacherScheduleData)) {
                _logger.LogInformation("Time Table is Created");
                int numberOfClasses = AllClasses.Count;
                for(int i = 0; i < numberOfClasses; i++)
                {
                    var ClassSchedule = new ClassScheduleEntity() { Class = AllClasses[i] };
                    for(int j = 0; j < 9; j++)
                    {
                        ClassSchedule.periods[j] = _context.Teachers.Find(ClassScheduleData[i][j]);
                    }
                    _context.classSchedule.Add(ClassSchedule);
                }
                _context.SaveChanges();
            }
            else
            {
                _logger.LogInformation("Time table wasn't created");
            }
                return Task.CompletedTask;
        }
    }
}

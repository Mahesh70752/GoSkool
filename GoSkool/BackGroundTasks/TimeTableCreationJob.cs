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

        public bool FillTimeTable(int i, int j, List<ClassEntity> AllClasses, List<List<int>> ClassScheduleData, Dictionary<int, List<int>> TeacherClassData, Dictionary<int, List<int>> TeacherScheduleData, List<int> ClassSubjectsCount)
        {
            if (j == 8)
            {
                return FillTimeTable(i + 1, 0, AllClasses, ClassScheduleData, TeacherClassData, TeacherScheduleData, ClassSubjectsCount);
            }
            if (i == AllClasses.Count)
            {
                return true;
            }
            var ClassId = AllClasses[i].Id;
            bool cur = false;
            if (9 - j > ClassSubjectsCount[i])
            {
                cur = FillTimeTable(i, j + 1, AllClasses, ClassScheduleData, TeacherClassData, TeacherScheduleData, ClassSubjectsCount);
            }
            if (cur) return true;
            foreach (var subject in AllClasses[i].Subjects)
            {
                var teacherId = subject.Teacher.Id;
                if (TeacherScheduleData[teacherId][j] == -1 && (!TeacherClassData[teacherId].Contains(ClassId)))
                {
                    TeacherClassData[teacherId].Add(ClassId);
                    ClassScheduleData[i][j] = teacherId;
                    TeacherScheduleData[teacherId][j] = ClassId;
                    ClassSubjectsCount[i]--;
                    if (FillTimeTable(i, j + 1, AllClasses, ClassScheduleData, TeacherClassData, TeacherScheduleData, ClassSubjectsCount)) return true;
                    ClassSubjectsCount[i]++;
                    TeacherClassData[teacherId].Remove(ClassId);
                    ClassScheduleData[i][j] = -1;
                    TeacherScheduleData[teacherId][j] = -1;
                }
            }
            return false;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _context.TeachersSchedule.ExecuteDelete();
            _context.TeacherScheduleClasses.ExecuteDelete();
            var AllClasses = _context.Classes.Include(cls => cls.Subjects).ThenInclude(sub => sub.Teacher).Include(cls => cls.Standard).Include(cls => cls.Section).ToList();
            var AllTeachers = _context.Teachers.Include(x => x.Classes).ToList();
            List<List<int>> ClassScheduleData = new List<List<int>>();
            Dictionary<int, List<int>> TeacherClassData = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> TeacherScheduleData = new Dictionary<int, List<int>>();
            List<int> ClassSubjectsCount = new List<int>();
            for (int i = 0; i < AllClasses.Count; i++)
            {
                ClassSubjectsCount.Add(AllClasses[i].Subjects.Count);
            }
            foreach (var Class in AllClasses)
            {
                ClassScheduleData.Add(new List<int>(8) { -1, -1, -1, -1, -1, -1, -1, -1 });
            }
            foreach (var teacher in AllTeachers)
            {
                TeacherClassData.Add(teacher.Id, new List<int>());
                TeacherScheduleData.Add(teacher.Id, new List<int>() { -1, -1, -1, -1, -1, -1, -1, -1 });
            }
            if (FillTimeTable(0, 0, AllClasses, ClassScheduleData, TeacherClassData, TeacherScheduleData, ClassSubjectsCount))
            {
                int numberOfClasses = AllClasses.Count;
                //for (int i = 0; i < numberOfClasses; i++)
                //{
                //    var ClassSchedule = new ClassScheduleEntity() { Class = AllClasses[i] };
                //    for (int j = 0; j < 8; j++)
                //    {
                //        if (ClassScheduleData[i][j] == -1)
                //        {
                //            ClassSchedule.periods.Add(_context.Teachers.Find(71));
                //            continue;
                //        }
                //        ClassSchedule.periods.Add(_context.Teachers.Find(ClassScheduleData[i][j]));
                //    }
                //    ClassSchedule.Id = 0;
                //    _context.Add(ClassSchedule);
                //}
                foreach (var teacher in AllTeachers)
                {
                    var teacherSchedule = new TeacherScheduleEntity() { Teacher = teacher };
                    _context.TeachersSchedule.Add(teacherSchedule);
                    _context.SaveChangesAsync().Wait();
                    for (int j = 0; j < 8; j++)
                    {
                        ClassEntity Class = null;
                        TeacherScheduleClassEntity tsc = null;
                        if (TeacherScheduleData[teacher.Id][j] == -1)
                        {
                            Class = _context.Classes.Find(82);
                        }
                        else
                        {
                            Class = _context.Classes.Find(TeacherScheduleData[teacher.Id][j]);
                        }
                            
                        tsc = new TeacherScheduleClassEntity() { Class = Class, TeacherSchedule = teacherSchedule };
                        _context.TeacherScheduleClasses.Add(tsc);
                        _context.SaveChangesAsync().Wait();
                        teacherSchedule.TeacherScheduleClasses.Add(tsc);
                    }
                    _context.TeachersSchedule.Update(teacherSchedule);
                }
                _context.SaveChanges();
                _logger.LogInformation("Time Table is Created");
            }
            else
            {
                _logger.LogInformation("Time table wasn't created");
            }
            return Task.CompletedTask;
        }
    }
}

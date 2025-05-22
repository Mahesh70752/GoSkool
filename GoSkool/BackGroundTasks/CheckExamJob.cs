using GoSkool.Data;
using GoSkool.Models;
using Quartz;

namespace GoSkool.BackGroundTasks
{
    public class CheckExamJob : IJob
    {
        private readonly ILogger<CheckExamJob> _logger;
        private readonly ApplicationDbContext _context;

        public CheckExamJob(ILogger<CheckExamJob> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var exams = new List<ExamEntity>();
            
            try
            {
                exams = _context.Exam.ToList();
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return Task.CompletedTask;
            }
            DateTime cur = DateTime.Now;
            foreach (var exam in exams) {
                if (exam.isCompleted)
                {
                    continue;
                }
                DateTime examTime = exam.ExamDate.AddMinutes(2);
                if(examTime<=DateTime.Now)
                {
                    Console.WriteLine("================================================");
                    Console.WriteLine("Exam: " + exam.Name +" completed.");
                    Console.WriteLine("================================================");
                    exam.isCompleted = true;
                    _context.Exam.Update(exam);
                }
            }
            _context.SaveChangesAsync().Wait();
            _logger.LogInformation("We are in check exam job");
            _logger.LogInformation(DateTime.Now.ToString());
            return Task.CompletedTask;
        }
    }
}

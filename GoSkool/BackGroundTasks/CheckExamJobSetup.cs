using Microsoft.Extensions.Options;
using Quartz;

namespace GoSkool.BackGroundTasks
{
    public class CheckExamJobSetup: IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(CheckExamJob));
            options
                .AddJob<CheckExamJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(trigger => trigger.ForJob(jobKey).WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10).RepeatForever()));
        }
    }
}

using Microsoft.Extensions.Options;
using Quartz;

namespace GoSkool.BackGroundTasks
{
    public class TimeTableCreationJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(TimeTableCreationJob));
            options
                .AddJob<TimeTableCreationJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(trigger => trigger.ForJob(jobKey).WithSimpleSchedule(schedule => schedule.WithIntervalInHours(24).RepeatForever()));
        }
    }
}

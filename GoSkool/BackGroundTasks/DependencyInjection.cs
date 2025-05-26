using Quartz;

namespace GoSkool.BackGroundTasks
{
    public static class DependencyInjection
    {
        public static void AddGoSkool(this IServiceCollection services)
        {
            services.AddQuartz();

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            //services.ConfigureOptions<TimeTableCreationJobSetup>();
            //services.ConfigureOptions<CheckExamJobSetup>();

        }
    }
}

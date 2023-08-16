using Microsoft.Extensions.Configuration;
using Quartz;
using System;

namespace ProductManagement.Extensions
{
    public static class ServiceCollectionQuartzConfiguratorExtensions
    {
        public static void AddJobAndTrigger<T>(
            this IServiceCollectionQuartzConfigurator quartz,
            IConfiguration config)
            where T : IJob
        {
            var jobKey = new JobKey("NotificationJob");
            var jobSchedule = int.Parse(config[$"AppSettings:NotificationRunEvery"]);

            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(Guid.NewGuid().ToString())
                .WithDescription("Notification Job")
                .WithSimpleSchedule(x => x
                                        .RepeatForever()
                                        .WithIntervalInMinutes(jobSchedule)));
        }
    }
}

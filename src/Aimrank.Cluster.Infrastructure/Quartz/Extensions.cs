using Aimrank.Cluster.Infrastructure.DataAccess;
using Aimrank.Cluster.Infrastructure.Processing.Inbox;
using Aimrank.Cluster.Infrastructure.Processing.Outbox;
using Aimrank.Cluster.Infrastructure.Processing.RemoveInactivePods;
using Aimrank.Cluster.Infrastructure.Processing.RemoveProcessedMessages;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using Quartz;
using System.Collections.Specialized;

namespace Aimrank.Cluster.Infrastructure.Quartz
{
    internal static class Extensions
    {
        private static IScheduler _scheduler;
        
        public static IServiceCollection AddQuartz(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<ClusterContext>()
                .AddClasses(classes => classes.AssignableTo<IJob>())
                .AsSelf()
                .WithTransientLifetime());

            return services;
        }

        public static IApplicationBuilder UseQuartz(this IApplicationBuilder builder)
        {
            var schedulerConfiguration = new NameValueCollection
            {
                {"quartz.scheduler.instanceName", "Aimrank.Cluster"}
            };

            var schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
            _scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            _scheduler.JobFactory = new JobFactory(builder.ApplicationServices);
            _scheduler.Start().GetAwaiter().GetResult();
            
            ScheduleCronJob<ProcessInboxJob>("0/5 * * ? * *");
            ScheduleCronJob<ProcessOutboxJob>("0/5 * * ? * *");
            ScheduleCronJob<RemoveInactivePodsJob>("0/30 * * ? * *");
            ScheduleCronJob<RemoveProcessedMessagesJob>("0 0 0/2 ? * *");

            return builder;
        }

        private static void ScheduleCronJob<T>(string cron) where T : class, IJob
        {
            var job = JobBuilder.Create<T>().Build();
            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithCronSchedule(cron)
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }
    }
}
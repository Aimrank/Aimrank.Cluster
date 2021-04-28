using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Quartz;
using System.Collections.Generic;
using System;

namespace Aimrank.Cluster.Infrastructure.Quartz
{
    internal class JobFactory : IJobFactory
    {
        private readonly Dictionary<IJob, IServiceScope> _scopes = new();
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope = _serviceProvider.CreateScope();
            var job = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            _scopes[job] = scope;
            return job;
        }

        public void ReturnJob(IJob job)
        {
            _scopes[job].Dispose();
            _scopes.Remove(job);
        }
    }
}
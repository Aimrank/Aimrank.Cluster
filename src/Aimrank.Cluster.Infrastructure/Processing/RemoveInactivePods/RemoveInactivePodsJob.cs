using Aimrank.Cluster.Core.Commands.RemoveInactivePods;
using Quartz;
using System.Threading.Tasks;

namespace Aimrank.Cluster.Infrastructure.Processing.RemoveInactivePods
{
    [DisallowConcurrentExecution]
    internal class RemoveInactivePodsJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
            => CommandsExecutor.Execute(new RemoveInactivePodsCommand());
    }
}
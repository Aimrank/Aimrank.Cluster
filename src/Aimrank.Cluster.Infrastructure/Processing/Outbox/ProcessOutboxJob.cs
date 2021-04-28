using System.Threading.Tasks;
using Quartz;

namespace Aimrank.Cluster.Infrastructure.Processing.Outbox
{
    [DisallowConcurrentExecution]
    internal class ProcessOutboxJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
            => CommandsExecutor.Execute(new ProcessOutboxCommand());
    }
}
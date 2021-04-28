using Quartz;
using System.Threading.Tasks;

namespace Aimrank.Cluster.Infrastructure.Processing.Inbox
{
    [DisallowConcurrentExecution]
    internal class ProcessInboxJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
            => CommandsExecutor.Execute(new ProcessInboxCommand());
    }
}
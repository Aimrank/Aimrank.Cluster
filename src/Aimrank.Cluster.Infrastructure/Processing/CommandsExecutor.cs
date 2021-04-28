using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;

namespace Aimrank.Cluster.Infrastructure.Processing
{
    internal static class CommandsExecutor
    {
        private static IServiceProvider _serviceProvider;

        public static void SetServiceProvider(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        public static async Task Execute(IRequest command)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(command);
        }
    }
}
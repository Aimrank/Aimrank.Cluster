using Aimrank.Cluster.Core.Events;
using Aimrank.Cluster.Infrastructure.DataAccess;
using Aimrank.Cluster.Infrastructure.Processing.Outbox;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Aimrank.Cluster.Infrastructure.Processing
{
    internal static class Extensions
    {
        public static IServiceCollection AddProcessing(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ClusterContext), typeof(IEvent));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPipelineBehavior<,>));
            services.AddScoped<IEventsDispatcher, OutboxEventsDispatcher>();

            return services;
        }

        public static IApplicationBuilder UseProcessing(this IApplicationBuilder builder)
        {
            CommandsExecutor.SetServiceProvider(builder.ApplicationServices);
            
            return builder;
        }
    }
}
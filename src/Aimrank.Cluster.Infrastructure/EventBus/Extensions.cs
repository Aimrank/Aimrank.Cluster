using Aimrank.Cluster.Infrastructure.EventBus.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aimrank.Cluster.Infrastructure.EventBus
{
    internal static class Extensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQSettings>(configuration.GetSection(nameof(RabbitMQSettings)));
            services.AddSingleton<IEventBus, RabbitMQEventBus>();
            services.AddSingleton<RabbitMQEventSerializer>();
            services.AddSingleton<RabbitMQRoutingKeyFactory>();
            services.AddHostedService<RabbitMQBackgroundService>();
            
            return services;
        }
    }
}
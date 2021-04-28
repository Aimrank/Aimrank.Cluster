using Aimrank.Cluster.Infrastructure.EventBus.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

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
        
        public static IApplicationBuilder UseEventBus(this IApplicationBuilder builder)
        {
            var backgroundService = builder.ApplicationServices
                .GetRequiredService<IEnumerable<IHostedService>>()
                .Single(s => s.GetType() == typeof(RabbitMQBackgroundService));

            if (backgroundService is RabbitMQBackgroundService service)
            {
                service.Configure();
            }
            
            return builder;
        }
    }
}
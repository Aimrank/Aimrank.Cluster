using Aimrank.Cluster.Core.Clients;
using Aimrank.Cluster.Infrastructure.DataAccess;
using Aimrank.Cluster.Infrastructure.EventBus;
using Aimrank.Cluster.Infrastructure.Processing;
using Aimrank.Cluster.Infrastructure.Quartz;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Aimrank.Cluster.Api")]
[assembly: InternalsVisibleTo("Aimrank.Cluster.Migrator")]

namespace Aimrank.Cluster.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccess(configuration);
            services.AddHttpClient();
            services.AddProcessing();
            services.AddQuartz();
            services.AddScoped<IPodClient, PodClient>();
            services.AddEventBus(configuration);
            
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
        {
            builder.UseProcessing();
            builder.UseQuartz();
            
            return builder;
        }
    }
}
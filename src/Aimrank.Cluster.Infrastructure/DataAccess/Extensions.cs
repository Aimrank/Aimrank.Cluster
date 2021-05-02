using Aimrank.Cluster.Core.Repositories;
using Aimrank.Cluster.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Aimrank.Cluster.Infrastructure.DataAccess
{
    internal static class Extensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ClusterContext>(options => options
                .UseNpgsql(configuration.GetConnectionString("Database"),
                    builder => builder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10), null))
                .UseSnakeCaseNamingConvention());
            
            services.Scan(scan => scan
                .FromAssemblyOf<ClusterContext>()
                .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Repository")))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
using Aimrank.Cluster.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;

namespace Aimrank.Cluster.Api
{
    public static class HostExtensions
    {
        public static async Task MigrateDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            
            var context = scope.ServiceProvider.GetRequiredService<ClusterContext>();
            
            var migrations = await context.Database.GetPendingMigrationsAsync();
            if (migrations.Any())
            {
                await context.Database.MigrateAsync();
            }
        }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neo4j.Migration;
using System.Threading.Tasks;

namespace Neo4jMigration.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task Migrate(this IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var migrate = scope.ServiceProvider.GetRequiredService<IMigrate>();
            var migrationConfiguration = scope.ServiceProvider.GetRequiredService<MigrationConfiguration>();
            await migrate.ExecuteAsync(migrationConfiguration);
        }
    }
}

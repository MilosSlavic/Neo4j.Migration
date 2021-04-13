using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Neo4j.Migration
{
    public static class MigrationConfigurationExtensions
    {
        public static async Task MigrateAsync(this MigrationConfiguration migrationConfiguration)
        {
            var loggerFactory = new LoggerFactory();
            var journalRepository = new Journal.JournalRepository(loggerFactory.CreateLogger<Journal.JournalRepository>(), migrationConfiguration.Driver);
            var migrate = new Migrate(loggerFactory.CreateLogger<Migrate>(), journalRepository);
            await migrate.ExecuteAsync(migrationConfiguration);
        }
    }
}

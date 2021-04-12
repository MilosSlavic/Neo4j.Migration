using System.Threading.Tasks;

namespace Neo4j.Migration
{
    public static class MigrationConfigurationExtensions
    {
        public static async Task MigrateAsync (this MigrationConfiguration migrationConfiguration)
        {
            var journalRepository = new Journal.JournalRepository(migrationConfiguration.Driver);
            var migrate = new Migrate(journalRepository);
            await migrate.ExecuteAsync(migrationConfiguration);
        }
    }
}

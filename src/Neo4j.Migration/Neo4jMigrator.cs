[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Neo4j.Migration.UnitTests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Neo4j.Migration.AspNetCore")]

namespace Neo4j.Migration
{
    public static class Neo4jMigrator
    {
       public static MigrationConfigurationBuilder Configuration => MigrationConfigurationBuilder.Create();
    }
}

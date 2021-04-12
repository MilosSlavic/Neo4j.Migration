using System.Reflection;

namespace Neo4j.Migration.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            Neo4jMigrator
                .Configuration
                .LoadEmbeddedScripts(Assembly.GetExecutingAssembly())
                .SetupDriver("bolt://neo4j", "neo4j", "P@ssw0rd")
                .Build()
                .MigrateAsync()
                .GetAwaiter()
                .GetResult();
        }
    }
}

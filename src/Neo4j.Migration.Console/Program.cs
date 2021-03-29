namespace Neo4j.Migration.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            Neo4jMigrator
                .Configuration
                .LoadEmbeddedScripts()
                .SetupDriver("neo4j url", "neo4j", "P@ssw0rd")
                .ExecuteAsync().GetAwaiter().GetResult();
        }
    }
}

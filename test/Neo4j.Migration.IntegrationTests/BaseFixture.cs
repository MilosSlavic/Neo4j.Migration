using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Neo4j.Driver;
using Neo4j.Migration.Journal;

namespace Neo4j.Migration.IntegrationTests
{
    public abstract class BaseFixture
    {
        public IDriver Driver { get; set; }
        public ILogger<JournalRepository> Logger { get; set; }
        public IConfiguration Configuration { get; set; }

        public BaseFixture()
        {
            Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false)
               .AddEnvironmentVariables()
               .Build();
            Logger = new Mock<ILogger<JournalRepository>>().Object;
            var neo4jUrl = Configuration.GetValue<string>("NEO4J_CONNECTION");
            var neo4jUser = Configuration.GetValue<string>("NEO4J_USERNAME");
            var neo4jPass = Configuration.GetValue<string>("NEO4J_PASSWORD");
            Driver = GraphDatabase.Driver(neo4jUrl, AuthTokens.Basic(neo4jUser, neo4jPass));
            JournalSeed.InitDb(Driver).GetAwaiter().GetResult();
        }
    }
}

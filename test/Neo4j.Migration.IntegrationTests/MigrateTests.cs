using Microsoft.Extensions.Configuration;
using Neo4j.Migration.Journal;
using System.Threading.Tasks;
using Xunit;

namespace Neo4j.Migration.IntegrationTests
{
    [Collection("MigrateDb")]
    public class MigrateTests
    {
        private readonly MigrateTestFixture _fixture;

        public MigrateTests(MigrateTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ApplyMigration()
        {
            var neo4jUrl = _fixture.Configuration.GetValue<string>("NEO4J_CONNECTION");
            var neo4jUser = _fixture.Configuration.GetValue<string>("NEO4J_USERNAME");
            var neo4jPass = _fixture.Configuration.GetValue<string>("NEO4J_PASSWORD");
            var config = Neo4jMigrator.Configuration
                .LoadEmbeddedScripts(this.GetType().Assembly)
                .SetupDriver(neo4jUrl, neo4jUser, neo4jPass)
                .Build();
            await config.MigrateAsync();

            var journalRepository = new JournalRepository(_fixture.Logger, _fixture.Driver);
            var lastVersion = await journalRepository.GetLastScriptAsync();
            Assert.Equal(3, lastVersion.Version);
        }
    }
}

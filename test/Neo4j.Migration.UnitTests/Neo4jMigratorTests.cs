using Xunit;

namespace Neo4j.Migration.UnitTests
{
    public class Neo4jMigratorTests
    {
        [Fact]
        public void Configuration_must_have_instance()
        {
            Assert.NotNull(Neo4jMigrator.Configuration);
        }
    }
}

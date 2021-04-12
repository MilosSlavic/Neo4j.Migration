using System.Linq;
using System.Reflection;
using Xunit;

namespace Neo4j.Migration.UnitTests
{
    public class EmbeddedResourceScriptLoaderTests
    {

        [Fact]
        public void ResolveVersion_success()
        {
            var loader = new EmbeddedResourceScriptLoader(Assembly.GetExecutingAssembly());
            var version = loader.ResolveVersion("0001_Initial.cypher");
            Assert.Equal(1, version);
        }

        [Fact]
        public void ParseStatements_success()
        {
            var loader = new EmbeddedResourceScriptLoader(Assembly.GetExecutingAssembly());
            var statement = "MATCH n RETURN n";
            var statements = loader.ParseStatements($"{statement};{statement}");
            Assert.NotEmpty(statements);
            Assert.Equal(2, statements.Count());
            Assert.True(statements.All(x => statement == x));
        }
    }
}

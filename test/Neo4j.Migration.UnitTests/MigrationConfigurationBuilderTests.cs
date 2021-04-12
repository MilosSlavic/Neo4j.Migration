using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Neo4j.Migration.UnitTests
{
    public class MigrationConfigurationBuilderTests
    {
        [Fact]
        public void Create_returns_new_instance()
        {
            var builder = MigrationConfigurationBuilder.Create();
            Assert.NotNull(builder);
        }

        [Fact]
        public void AddScriptLoader_throw_ArgumentNullException()
        {
            var builder = MigrationConfigurationBuilder.Create();
            Assert.Throws<ArgumentNullException>(() => builder.AddScriptLoader(null));
        }

        [Fact]
        public void AddScriptLoader_success()
        {
            var builder = MigrationConfigurationBuilder.Create();
            var scriptLoaderMock = new Mock<IScriptLoader>();

            builder.AddScriptLoader(scriptLoaderMock.Object);

            Assert.Equal(scriptLoaderMock.Object.GetHashCode(), builder.Build().ScriptLoaders.First().GetHashCode());
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("url", null, null)]
        [InlineData("url", "username", null)]
        public void SetupDriver_BasicAuth_throws_ArgumentNullException(string url, string username, string password)
        {
            Assert.Throws<ArgumentNullException>(() => MigrationConfigurationBuilder.Create().SetupDriver(url, username, password));
        }

        [Fact]
        public void SetupDriver_BasicAuth_success()
        {
            var configuration = MigrationConfigurationBuilder
                .Create()
                .SetupDriver("bolt://uri/", "user", "pass")
                .Build();

            Assert.NotNull(configuration.Driver);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("url", null)]
        public void SetupDriver_Kerberos_throws_ArgumentNullExceoption(string url, string base64KerberosToken)
        {
            Assert.Throws<ArgumentNullException>(() => MigrationConfigurationBuilder.Create().SetupDriver(url, base64KerberosToken));
        }

        [Fact]
        public void SetupDriver_Kerberos_success()
        {
            var configuration = MigrationConfigurationBuilder
                .Create()
                .SetupDriver("bolt://uri/", "base64KerberosToken")
                .Build();

            Assert.NotNull(configuration.Driver);
        }

        [Fact]
        public void Build_returns_configuration_instance()
        {
            var configuration = MigrationConfigurationBuilder.Create().Build();
            Assert.NotNull(configuration);
        }

        // TODO: Execute async
    }
}

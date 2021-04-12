using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Neo4j.Migration.UnitTests
{
    public class ScriptLoaderExtensionsTests
    {
        [Fact]
        public void AddsEmbeddedScriptLoader()
        {
            var configuration = MigrationConfigurationBuilder
                .Create()
                .LoadEmbeddedScripts(Assembly.GetExecutingAssembly())
                .Build();
            Assert.True(configuration.ScriptLoaders.Any());
        }

        [Fact]
        public void AddsEmbeddedScriptLoader_with_options()
        {
            var configuration = MigrationConfigurationBuilder
                .Create()
                .LoadEmbeddedScripts(Assembly.GetExecutingAssembly(), opt =>
                {
                    opt.Delimiter = ',';
                    opt.FileExtension = "*.cypher";
                })
                .Build();
            Assert.True(configuration.ScriptLoaders.Any());
        }

        [Fact]
        public void AddsEmbeddedScriptLoader_multiple_assemblies()
        {
            var configuration = MigrationConfigurationBuilder
                .Create()
                .LoadEmbeddedScripts(new[] { Assembly.GetExecutingAssembly() })
                .Build();
            Assert.True(configuration.ScriptLoaders.Any());
        }

        [Fact]
        public void AddsEmbeddedScriptLoader_with_options_and_multiple_assemblies()
        {
            var configuration = MigrationConfigurationBuilder
                .Create()
                .LoadEmbeddedScripts(new[] { Assembly.GetExecutingAssembly() }, opt =>
                {
                    opt.Delimiter = ',';
                    opt.FileExtension = "*.cypher";
                })
                .Build();
            Assert.True(configuration.ScriptLoaders.Any());
        }
    }
}

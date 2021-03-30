using System;
using System.Reflection;

namespace Neo4j.Migration
{
    public static class ScriptLoaderExtensions
    {
        public static MigrationConfigurationBuilder LoadEmbeddedScripts(this MigrationConfigurationBuilder builder, Assembly assembly)
        {
            return builder.LoadEmbeddedScripts(assembly, null);
        }

        public static MigrationConfigurationBuilder LoadEmbeddedScripts(this MigrationConfigurationBuilder builder, Assembly assembly, Action<EmbeddedResourceOptions> options)
        {
            return builder.AddScriptLoader(new EmbeddedResourceScriptLoader(assembly, options));
        }

        public static MigrationConfigurationBuilder LoadEmbeddedScripts(this MigrationConfigurationBuilder builder, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                builder.LoadEmbeddedScripts(assembly);
            }
            return builder;
        }

        public static MigrationConfigurationBuilder LoadEmbeddedScripts(this MigrationConfigurationBuilder builder, Assembly[] assemblies, Action<EmbeddedResourceOptions> options)
        {
            foreach (var assembly in assemblies)
            {
                builder.LoadEmbeddedScripts(assembly, options);
            }
            return builder;
        }
    }
}

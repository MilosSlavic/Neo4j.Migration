using System;
using System.Collections.Generic;
using System.Text;

namespace Neo4j.Migration
{
    public static class ScriptLoaderExtensions
    {
        public static MigrationConfigurationBuilder LoadEmbeddedScripts(this MigrationConfigurationBuilder builder)
        {
            return builder.AddScriptLoader(new EmbeddedResourceScriptLoader());
        }
    }
}

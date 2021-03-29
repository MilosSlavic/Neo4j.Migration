using Neo4j.Driver;
using System.Collections.Generic;

namespace Neo4j.Migration
{
    public class MigrationConfiguration
    {
        internal List<IScriptLoader> ScriptLoaders { get; set; } = new List<IScriptLoader>();

        internal string ConnectionString { get; set; }

        internal IDriver Driver { get; set; }
    }
}

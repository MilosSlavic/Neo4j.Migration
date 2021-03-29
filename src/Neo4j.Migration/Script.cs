using System.Collections.Generic;

namespace Neo4j.Migration
{
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public class Script
    {
        public int Version { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Statements { get; set; }
    }
}

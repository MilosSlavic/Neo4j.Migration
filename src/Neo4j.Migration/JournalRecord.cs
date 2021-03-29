using System;

namespace Neo4j.Migration
{
    public class JournalRecord
    {
        public int Version { get; set; }

        public string ScriptName { get; set; }

        public DateTime AppliedAt { get; set; }
    }
}

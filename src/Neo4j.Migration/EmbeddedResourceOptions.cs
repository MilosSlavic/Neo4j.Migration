﻿namespace Neo4j.Migration
{
    public class EmbeddedResourceOptions
    {
        public string FileExtension { get; set; } = "cypher";

        public char Delimiter { get; set; } = ';';
    }
}

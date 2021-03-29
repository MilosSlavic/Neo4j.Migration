using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.Migration
{
    public class EmbeddedResourceScriptLoader : IScriptLoader
    {
        public Task<IEnumerable<Script>> LoadAsync(int lastVersion = 0)
        {
            throw new NotImplementedException();
        }
    }
}

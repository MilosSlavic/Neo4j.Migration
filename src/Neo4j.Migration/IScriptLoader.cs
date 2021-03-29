using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neo4j.Migration
{
    public interface IScriptLoader
    {
        Task<IEnumerable<Script>> LoadAsync(int lastVersion = 0);
    }
}

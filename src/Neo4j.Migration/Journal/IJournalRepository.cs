using Neo4j.Driver;
using System.Threading.Tasks;

namespace Neo4j.Migration.Journal
{
    public interface IJournalRepository
    {
        Task<JournalRecord> GetLastScriptAsync(IAsyncTransaction transaction = null);

        Task AddAsync(JournalRecord record, IAsyncTransaction transaction = null);
    }
}

using System.Threading.Tasks;

namespace Neo4j.Migration
{
    public interface IJournalRepository
    {
        Task<JournalRecord> GetLastScriptAsync();

        Task AddAsync(JournalRecord record);
    }
}

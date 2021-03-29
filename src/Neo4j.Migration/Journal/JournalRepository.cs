using Neo4j.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j.Migration.Journal
{
    public class JournalRepository : IJournalRepository
    {
        private readonly IDriver _driver;

        public JournalRepository(
            IDriver driver)
        {
            _driver = driver;
        }

        public async Task AddAsync(JournalRecord record)
        {
            const string query = @"";
            var session = _driver.AsyncSession();
            try
            {
                await session.WriteTransactionAsync(async tx =>
                {
                    await tx.RunAsync(query, record);
                    await tx.CommitAsync();
                });
            }
            catch (Exception)
            {
                // TODO: Logging
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<JournalRecord> GetLastScriptAsync()
        {
            const string query = @"
                MATCH (j:MigrationJournal)
                ORDER BY j.Version DESC
                LIMIT 1";
            var session = _driver.AsyncSession();
            try
            {
                return await session.ReadTransactionAsync(async tx =>
                {
                    var cursor = await tx.RunAsync(query);
                    var records = await cursor.ToListAsync();
                    var record = records.SingleOrDefault();
                    if (record is null)
                    {
                        return null;
                    }

                    return new JournalRecord
                    {
                        Version = record["Version"].As<int>(),
                        ScriptName = record["ScriptName"].As<string>(),
                        AppliedAt = record["AppliedAt"].As<DateTime>()
                    };
                });
            }
            catch (Exception)
            {
                // TODO: Logging
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}

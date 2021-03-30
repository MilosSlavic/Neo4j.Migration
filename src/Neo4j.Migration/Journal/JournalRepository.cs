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

        public async Task AddAsync(JournalRecord record, IAsyncTransaction transaction = null)
        {
            const string query = @"CREATE (j:MigrationJournal { Version: $Version, ScriptName: $ScriptName, AppliedAt: $AppliedAt})";
            var addFunc = new Func<IAsyncTransaction, Task>(async (tx) =>
            {
                await tx.RunAsync(query, record);
                await tx.CommitAsync();
            });

            if (transaction is null)
            {
                await AddInSessionAsync(addFunc);
                return;
            }

            await addFunc(transaction);
        }

        private async Task AddInSessionAsync(Func<IAsyncTransaction, Task> func)
        {
            var session = _driver.AsyncSession();
            try
            {
                await session.WriteTransactionAsync(func);
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

        public async Task<JournalRecord> GetLastScriptAsync(IAsyncTransaction transaction = null)
        {
            const string query = @"
                MATCH (j:MigrationJournal)
                ORDER BY j.Version DESC
                LIMIT 1";
            var getLastScriptFunc = new Func<IAsyncTransaction, Task<JournalRecord>>(async tx =>
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

            if (transaction is null)
            {
                return await GetLastScriptInSessionAsync(getLastScriptFunc);
            }

            return await getLastScriptFunc(transaction);
        }

        private async Task<JournalRecord> GetLastScriptInSessionAsync(Func<IAsyncTransaction, Task<JournalRecord>> func)
        {
            var session = _driver.AsyncSession();
            try
            {
                return await session.ReadTransactionAsync(func);
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

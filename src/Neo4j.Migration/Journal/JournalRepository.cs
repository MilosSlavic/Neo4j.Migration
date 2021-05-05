using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j.Migration.Journal
{
    public class JournalRepository : IJournalRepository, IDisposable
    {
        private readonly ILogger<JournalRepository> _logger;
        private IDriver _driver;
        private bool _disposedValue;

        public JournalRepository(
            ILogger<JournalRepository> logger,
            IDriver driver)
        {
            _logger = logger;
            _driver = driver;
        }

        public async Task AddAsync(JournalRecord record)
        {
            _logger.LogDebug($"Writing new journal record. Version: '{record.Version}' Name: '{record.ScriptName}'.");
            const string query = @"CREATE (j:MigrationJournal { Version: $Version, ScriptName: $ScriptName, AppliedAt: $AppliedAt})";

            await AddInSessionAsync(async (tx) =>
            {
                await tx.RunAsync(query, record);
                await tx.CommitAsync();
            });
        }

        private async Task AddInSessionAsync(Func<IAsyncTransaction, Task> func)
        {
            var session = _driver.AsyncSession();
            try
            {
                await session.WriteTransactionAsync(func);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<JournalRecord> GetLastScriptAsync()
        {
            _logger.LogInformation("Fetching the last applied script information.");
            const string query = @"
                MATCH (j:MigrationJournal)
                RETURN j.Version as Version, j.ScriptName as ScriptName, j.AppliedAt as AppliedAt
                ORDER BY j.Version DESC
                LIMIT 1";

            return await GetLastScriptInSessionAsync(async tx =>
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

        private async Task<JournalRecord> GetLastScriptInSessionAsync(Func<IAsyncTransaction, Task<JournalRecord>> func)
        {
            var session = _driver.AsyncSession();
            try
            {
                return await session.ReadTransactionAsync(func);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _driver = null;
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

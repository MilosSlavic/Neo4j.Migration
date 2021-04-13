using Neo4j.Driver;
using Neo4j.Migration.Journal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neo4j.Migration.IntegrationTests
{
    public class JournalSeed
    {
        public static async Task InitDb(IDriver driver)
        {
            var journalRecords = GetSeedData();
            var session = driver.AsyncSession();

            try
            {
                await session.WriteTransactionAsync(async x =>
                {
                    foreach (var journalRecord in journalRecords)
                    {
                        const string query = "CREATE (j:MigrationJournal { Version: $Version, ScriptName: $ScriptName, AppliedAt: $AppliedAt})";
                        await x.RunAsync(query, journalRecord);
                    }
                    await x.CommitAsync();
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        private static IEnumerable<JournalRecord> GetSeedData() => new List<JournalRecord>
        {
            new JournalRecord
            {
                Version = 1,
                ScriptName = "0001_init",
                AppliedAt= DateTime.Now,
            },
            new JournalRecord
            {
                Version = 2,
                ScriptName = "0002_indexes",
                AppliedAt = DateTime.Now
            }
        };

        public static async Task CleanDb(IDriver driver)
        {
            var session = driver.AsyncSession();

            try
            {
                await session.WriteTransactionAsync(async x =>
                {
                    await x.RunAsync("MATCH (n) DETACH DELETE n");
                    await x.CommitAsync();
                });
            }
            finally
            {
                await session.CloseAsync();
            }

            await driver.CloseAsync();
        }
    }
}

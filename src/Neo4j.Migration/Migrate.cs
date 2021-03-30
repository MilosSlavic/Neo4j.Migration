using Neo4j.Migration.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j.Migration
{
    internal sealed class Migrate : IMigrate
    {
        private readonly IJournalRepository _journalRepository;

        public Migrate(
            IJournalRepository journalRepository)
        {
            _journalRepository = journalRepository;
        }

        public async Task ExecuteAsync(MigrationConfiguration configuration)
        {
            var lastVersion = await _journalRepository.GetLastScriptAsync();
            var scripts = new List<Script>();
            await foreach (var scriptLoaderResult in GetScriptsAsync(configuration.ScriptLoaders, lastVersion.Version))
            {
                scripts.AddRange(scriptLoaderResult);
            }

            await ExecuteSequentialyAsync(configuration, scripts);
        }

        private async IAsyncEnumerable<IEnumerable<Script>> GetScriptsAsync(ICollection<IScriptLoader> scriptLoaders, int lastVersion)
        {
            foreach (var scriptLoader in scriptLoaders)
            {
                yield return await scriptLoader.LoadAsync(lastVersion);
            }
        }

        private async Task ExecuteSequentialyAsync(MigrationConfiguration configuration, ICollection<Script> scripts)
        {
            var orderedScripts = scripts.OrderBy(x => x.Version).ToList();
            foreach (var script in orderedScripts)
            {
                var session = configuration.Driver.AsyncSession();
                try
                {
                    await session.WriteTransactionAsync(async tx =>
                    {
                        foreach (var statement in script.Statements)
                        {
                            await tx.RunAsync(statement);
                        }

                        var journalRecord = new JournalRecord
                        {
                            Version = script.Version,
                            ScriptName = script.Name,
                            AppliedAt = DateTime.UtcNow
                        };
                        await _journalRepository.AddAsync(journalRecord);
                        await tx.CommitAsync();
                    });
                }
                catch (Exception)
                {
                    // TODO: Logging
                    break;
                }
                finally
                {
                    await session.CloseAsync();
                }
            }
        }
    }
}

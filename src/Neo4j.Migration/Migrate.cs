using Microsoft.Extensions.Logging;
using Neo4j.Migration.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j.Migration
{
    internal sealed class Migrate : IMigrate
    {
        private readonly ILogger<Migrate> _logger;
        private readonly IJournalRepository _journalRepository;

        public Migrate(
            ILogger<Migrate> logger,
            IJournalRepository journalRepository)
        {
            _logger = logger;
            _journalRepository = journalRepository;
        }

        public async Task ExecuteAsync(MigrationConfiguration configuration)
        {
            _logger.LogInformation("Started appling migrations");
            int lastVersion = 0;
            var lastJournalRecord = await _journalRepository.GetLastScriptAsync();
            if (lastJournalRecord is not null)
            {
                lastVersion = lastJournalRecord.Version;
            }

            _logger.LogInformation($"Resolved the last version: '{lastVersion}'");
            var scripts = new List<Script>();
            await foreach (var scriptLoaderResult in GetScriptsAsync(configuration.ScriptLoaders, lastVersion))
            {
                scripts.AddRange(scriptLoaderResult);
            }

            _logger.LogInformation($"Found {scripts.Count} scripts");
            _logger.LogInformation("Executing sequentially. . .");
            await ExecuteSequentialyAsync(configuration, scripts);
            _logger.LogInformation("Executed successfully!");
        }

        internal async IAsyncEnumerable<IEnumerable<Script>> GetScriptsAsync(ICollection<IScriptLoader> scriptLoaders, int lastVersion)
        {
            foreach (var scriptLoader in scriptLoaders)
            {
                yield return await scriptLoader.LoadAsync(lastVersion);
            }
        }

        internal async Task ExecuteSequentialyAsync(MigrationConfiguration configuration, ICollection<Script> scripts)
        {
            var orderedScripts = scripts.OrderBy(x => x.Version).ToList();
            foreach (var script in orderedScripts)
            {
                _logger.LogInformation($"Script info - Name'{script.Name}' Version '{script.Version}'");
                var session = configuration.Driver.AsyncSession();
                try
                {
                    await session.WriteTransactionAsync(async tx =>
                    {
                        foreach (var statement in script.Statements)
                        {
                            if (string.IsNullOrEmpty(statement))
                            {
                                continue;
                            }

                            _logger.LogDebug($"Executing statement: \n{statement}\n");
                            await tx.RunAsync(statement);
                        }
                    });

                    await _journalRepository.AddAsync(new JournalRecord
                    {
                        Version = script.Version,
                        ScriptName = script.Name,
                        AppliedAt = DateTime.Now
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
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

using Neo4j.Migration.Journal;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Neo4j.Migration.IntegrationTests
{
    [Collection("JournalDb")]
    public class JournalRepositoryTests
    {
        private readonly IJournalRepository _journalRepository;
        private readonly JournalTestFixture _fixture;

        public JournalRepositoryTests(JournalTestFixture fixture)
        {
            _fixture = fixture;
            _journalRepository = new JournalRepository(_fixture.Logger, _fixture.Driver);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            var journalRecord = new JournalRecord
            {
                AppliedAt = DateTime.Now,
                ScriptName = "Test",
                Version = 3
            };
            await _journalRepository.AddAsync(journalRecord);
            var lastVersion = await _journalRepository.GetLastScriptAsync();
            Assert.Equal(journalRecord.Version, lastVersion.Version);
        }
    }
}

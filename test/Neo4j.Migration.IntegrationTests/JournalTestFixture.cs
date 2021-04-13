using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Neo4j.Driver;
using Neo4j.Migration.Journal;
using System;
using Xunit;

namespace Neo4j.Migration.IntegrationTests
{
    public class JournalTestFixture : BaseFixture, IDisposable
    {
        private bool _disposedValue;
        
        public JournalTestFixture()
            : base()
        {
            JournalSeed.InitDb(Driver).GetAwaiter().GetResult();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    JournalSeed.CleanDb(Driver).GetAwaiter().GetResult();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    [CollectionDefinition("JournalDb")]
    public class JournalDataBaseCollection : ICollectionFixture<JournalTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}

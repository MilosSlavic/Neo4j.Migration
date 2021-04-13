using System;
using Xunit;

namespace Neo4j.Migration.IntegrationTests
{
    public class MigrateTestFixture : BaseFixture, IDisposable
    {
        private bool _disposedValue;

        public MigrateTestFixture()
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

    [CollectionDefinition("MigrateDb")]
    public class MigrateDatabaseCollection : ICollectionFixture<MigrateTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}

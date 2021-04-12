using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neo4j.Migration
{
    public class MigrationConfiguration : IDisposable, IAsyncDisposable
    {
        private bool _disposedValue;

        internal List<IScriptLoader> ScriptLoaders { get; set; } = new List<IScriptLoader>();

        internal IDriver Driver { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Driver.CloseAsync().GetAwaiter().GetResult();
                    Driver.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await Driver.CloseAsync();
            Driver.Dispose();
        }
    }
}

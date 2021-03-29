using Neo4j.Driver;
using System;
using System.Threading.Tasks;

namespace Neo4j.Migration
{
    public class MigrationConfigurationBuilder
    {
        private readonly MigrationConfiguration _configuration;

        private MigrationConfigurationBuilder()
        {
            _configuration = new MigrationConfiguration();
        }

        public static MigrationConfigurationBuilder Create()
        {
            return new MigrationConfigurationBuilder();
        }

        public MigrationConfigurationBuilder AddScriptLoader(IScriptLoader scriptLoader)
        {
            if (scriptLoader is null)
            {
                throw new ArgumentNullException(nameof(scriptLoader));
            }

            _configuration.ScriptLoaders.Add(scriptLoader);
            return this;
        }

        public MigrationConfigurationBuilder SetupDriver(string url, string username, string password)
        {
            // TODO: Add validation
            _configuration.Driver = GraphDatabase.Driver(url, AuthTokens.Basic(username, password));
            return this;
        }

        public MigrationConfigurationBuilder SetupDriver(string url, string base64KerberosToken)
        {
            // TODO: Add validation
            _configuration.Driver = GraphDatabase.Driver(url, AuthTokens.Kerberos(base64KerberosToken));
            return this;
        }

        internal MigrationConfiguration Build()
        {
            return _configuration;
        }

        public Task ExecuteAsync()
        {
            var config = this.Build();
            throw new NotImplementedException();
        }
    }
}

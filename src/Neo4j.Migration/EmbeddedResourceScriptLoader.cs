using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Neo4j.Migration
{
    public class EmbeddedResourceScriptLoader : IScriptLoader
    {
        private readonly Assembly _assembly;
        private readonly Action<EmbeddedResourceOptions> _userOptions;
        private readonly EmbeddedResourceOptions _options;

        public EmbeddedResourceScriptLoader(Assembly assembly, Action<EmbeddedResourceOptions> userOptions = null)
        {
            _assembly = assembly;
            _userOptions = userOptions;
            _options = new EmbeddedResourceOptions();
        }

        public async Task<IEnumerable<Script>> LoadAsync(int lastVersion = 0)
        {
            if (_userOptions != null)
            {
                _userOptions.Invoke(_options);
            }

            var scriptNames = _assembly
                .GetManifestResourceNames()
                .Where(x => x.EndsWith(_options.FileExtension))
                .Where(x => ResolveVersion(x) > lastVersion);

            var result = new List<Script>(scriptNames.Count());
            foreach (var scriptName in scriptNames)
            {
                using var stream = _assembly.GetManifestResourceStream(scriptName);
                using var streamReader = new StreamReader(stream);
                var content = await streamReader.ReadToEndAsync();
                var script = new Script
                {
                    Name = scriptName,
                    Version = ResolveVersion(scriptName),
                    Statements = ParseStatements(content)
                };
                result.Add(script);
            }

            return result;
        }

        public int ResolveVersion(string scriptName) => Convert.ToInt32(scriptName.Split('_')[0]);

        public IEnumerable<string> ParseStatements(string content) => content.Split(_options.Delimiter).Select(x => x.Trim());
    }
}

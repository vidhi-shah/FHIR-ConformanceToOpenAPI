using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Documentation.Markdown.Variable
{
    public class VariableDictionaryFileLoader : IVariableDictionaryLoader
    {
        private string _variableConfigPath;

        public VariableDictionaryFileLoader(string variableConfigPath)
        {
            _variableConfigPath = variableConfigPath;
        }

        public IDictionary<string, string> Load()
        {
            // all these things need to be passed in. no hardcoding look at other loaders. same as markdown
            var jsonString = File.ReadAllText(_variableConfigPath);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
        }
    }
}

using System.Collections.Generic;

namespace Documentation.Markdown.Variable
{
    public interface IVariableDictionaryLoader
    {
        IDictionary<string, string> Load();
    }
}

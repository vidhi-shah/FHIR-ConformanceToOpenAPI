using System;
using System.Collections.Generic;

namespace Documentation.Markdown.Variable
{
    public class ReplaceVariableOperation
    {
        public ReplaceVariableOperation(IVariableDictionaryLoader variableDictionaryLoader)
        {
            Variables = variableDictionaryLoader.Load();
        }

        private IDictionary<string, string> Variables { get; }

        public IMarkdownData[] Execute(IMarkdownData[] mds)
        {
            var list = new List<IMarkdownData>();

            foreach (var md in mds)
            {
                list.Add(Execute(md));
            }

            return list.ToArray();
        }

        public IMarkdownData Execute(IMarkdownData markdown)
        {
            markdown.Content = Execute(markdown.Content);
            return markdown;
        }

        public string Execute(string content)
        {
            foreach (var variable in Variables)
            {
                content = content.Replace(variable.Key, variable.Value, StringComparison.InvariantCulture);
            }

            return content;
        }
    }
}

using Documentation.Markdown;
using Documentation.Markdown.Variable;
using Microsoft.OpenApi.Models;

namespace Documentation.OpenApi
{
    public class MarkdownOpenApiDocumentGenerator : IMarkdownDocumentGenerator
    {
        private IMarkdownLoader _markdownLoader;
        private IVariableDictionaryLoader _variableDictionaryLoader;

        public MarkdownOpenApiDocumentGenerator(IMarkdownLoader markdownLoader, IVariableDictionaryLoader variableDictionaryLoader)
        {
            _markdownLoader = markdownLoader;
            _variableDictionaryLoader = variableDictionaryLoader;
        }

        public OpenApiDocument Generate()
        {
            var openApiDocument = new OpenApiDocument();
            var fileSystemMarkdowns = _markdownLoader.Load();
            var markDownData = new ReplaceVariableOperation(_variableDictionaryLoader).Execute(fileSystemMarkdowns);

            foreach (var markDownFile in markDownData)
            {
                openApiDocument.Tags.Add(MarkdownData.ToOpenApiTag(markDownFile));
            }

            return openApiDocument;
        }
    }
}

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Documentation.Markdown;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;

namespace Documentation.OpenApi
{
    public static class OpenApiDocumentExtensions
    {
        public static OpenApiDocument Load(this OpenApiDocument doc, IMarkdownLoader markdownLoader)
        {
            var mds = markdownLoader.Load();

            foreach (var md in mds.OrderBy(f => f.Sequence))
            {
                doc.AddMarkdownAsTag(md);
            }

            return doc;
        }

        public static OpenApiDocument AddMarkdownAsTag(this OpenApiDocument doc, IMarkdownData md)
        {
            var tag = MarkdownData.ToOpenApiTag(md);
            doc.Tags.Add(tag);
            return doc;
        }

        // arguments can be in an object if there is more.  there should maybe be a an object loader. same as markdown
        public static OpenApiDocument SetInfo(this OpenApiDocument doc, IDocumentInfo swaggerInfo)
        {
            doc.Info = DocumentInfo.ToOpenApiInfo(swaggerInfo);

            return doc;
        }

        public static async Task SaveAsync(OpenApiDocument openApiDocument, string filePath)
        {
            using (var outputString = new StringWriter())
            {
                var writer = new OpenApiJsonWriter(outputString);
                openApiDocument.SerializeAsV3(writer);
                await File.WriteAllTextAsync(filePath, outputString.ToString());
            }
        }
    }
}

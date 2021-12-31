using Microsoft.OpenApi.Models;

namespace Documentation.OpenApi
{
    public interface IMarkdownDocumentGenerator
    {
        OpenApiDocument Generate();
    }
}

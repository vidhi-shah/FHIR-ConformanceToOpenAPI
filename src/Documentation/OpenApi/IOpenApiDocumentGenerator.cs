using System.Threading.Tasks;
using Microsoft.OpenApi.Models;

namespace Documentation.OpenApi
{
    public interface IOpenApiDocumentGenerator
    {
        Task<OpenApiDocument> GenerateAsync(string hostUrl);
    }
}

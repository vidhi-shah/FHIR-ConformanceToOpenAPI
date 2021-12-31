using System.Threading.Tasks;
using Documentation.OpenApi;
using NUnit.Framework;

namespace Documentation.Tests
{
    public class ConformanceToOpenApiDocumentGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Explicit]
        public async Task ConformanceToOpenApiDocumentGeneratorTest()
        {
            var hosturl = "https://fhir.lkclouddev.com/R4";
            var authUrl = "https://localhost:44336/connect/authorize";
            var tokenUrl = "https://localhost:44336/connect/token";
            var outputSwaggerPath = @"D:\conformanceswagger.json";

            var openApiDoc = await new OpenApi.ConformanceToOpenApiDocumentGenerator(authUrl, tokenUrl).GenerateAsync(hosturl);
            await OpenApiDocumentExtensions.SaveAsync(openApiDoc, outputSwaggerPath);

            Assert.Pass();
        }
    }
}

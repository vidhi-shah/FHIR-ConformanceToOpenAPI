using System.Threading.Tasks;
using Documentation.Markdown;
using Documentation.Markdown.Variable;
using Documentation.OpenApi;
using NUnit.Framework;

namespace Documentation.Tests
{
    public class MarkdownDocumentGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Explicit]
        public async Task MarkDownOpenApiDocumentGeneratorTest()
        {
            var directoryPath = @"D:\LKFHIR\LKFhir\src\LKFHIR\LKFhir.Documentation\Documentation\";
            var variableConfigPath = @"D:\LKFHIR\LKFhir\src\LKFHIR\Ellkay.Documentation.Tests\variableConfig.json";
            var outputSwaggerPath = @"D:\openapidocsswagger.json";
            var markdownLoader = new FileSystemMarkdownLoader(new System.IO.DirectoryInfo(directoryPath));
            var variableLoader = new VariableDictionaryFileLoader(variableConfigPath);
            var openApiDoc = new OpenApi.MarkdownOpenApiDocumentGenerator(markdownLoader, variableLoader).Generate();
            await OpenApiDocumentExtensions.SaveAsync(openApiDoc, outputSwaggerPath);

            Assert.Pass();
        }
    }
}

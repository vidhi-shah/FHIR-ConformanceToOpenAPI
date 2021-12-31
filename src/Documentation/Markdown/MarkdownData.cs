using Microsoft.OpenApi.Models;

namespace Documentation.Markdown
{
    public class MarkdownData : IMarkdownData
    {
        public int Sequence { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public static OpenApiTag ToOpenApiTag(IMarkdownData file)
        {
            return new OpenApiTag
            {
                Name = file.Title,
                Description = file.Content,
            };
        }
    }
}

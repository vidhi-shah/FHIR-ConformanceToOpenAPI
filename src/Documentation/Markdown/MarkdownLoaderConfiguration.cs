using System.Collections.Generic;
using System.IO;

namespace Documentation.Markdown
{
    public class MarkdownLoaderConfiguration
    {
        public DirectoryInfo Directory { get; set; }

        public ICollection<MarkdownConfiguration> Files { get; }
    }
}

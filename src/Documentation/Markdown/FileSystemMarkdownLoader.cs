using System;
using System.IO;
using System.Linq;

namespace Documentation.Markdown
{
    public class FileSystemMarkdownLoader : IMarkdownLoader
    {
        private DirectoryInfo _directory;

        public FileSystemMarkdownLoader(DirectoryInfo directory)
        {
            _directory = directory;
        }

        public IMarkdownData[] Load()
        {
            return _directory
                .GetFiles()
                .Where(f => f.Extension == ".md")
                .Select(f => Load(f.FullName))
                .ToArray();
        }

        private static IMarkdownData Load(string path)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);

            var (sequence, name) = ParseFileName(fileNameWithoutExtension);

            var markdownFile = new MarkdownData
            {
                Sequence = sequence,
                Title = name,
                Content = File.ReadAllText(path),
            };

            return markdownFile;
        }

        private static (int sequence, string name) ParseFileName(string filename)
        {
            var split = filename.Split('-', StringSplitOptions.None);
            var sequence = int.Parse(split[0]);
            var name = split[1].Replace('_', ' ');
            return (sequence, name);
        }
    }
}

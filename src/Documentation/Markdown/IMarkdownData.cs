namespace Documentation.Markdown
{
    public interface IMarkdownData
    {
        public int Sequence { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}

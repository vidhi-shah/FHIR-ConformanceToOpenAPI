namespace Documentation.OpenApi
{
    public interface IDocumentInfo
    {
        public string Version { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }

        public string LogoAltText { get; set; }
    }
}

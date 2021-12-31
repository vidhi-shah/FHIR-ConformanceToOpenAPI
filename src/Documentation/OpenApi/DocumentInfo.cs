using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

namespace Documentation.OpenApi
{
    public class DocumentInfo : IDocumentInfo
    {
        public string Version { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }

        public string LogoAltText { get; set; }

        public static OpenApiInfo ToOpenApiInfo(IDocumentInfo swaggerInfo)
        {
            return new OpenApiInfo
            {
                Version = swaggerInfo.Version,
                Title = swaggerInfo.Title,
                Description = swaggerInfo.Description,
                Extensions = AddLogo(swaggerInfo.Logo, swaggerInfo.LogoAltText),
            };
        }

        private static Dictionary<string, IOpenApiExtension> AddLogo(string logo, string logoAltText)
        {
            return new Dictionary<string, IOpenApiExtension>
            {
              {
                "x-logo", new OpenApiObject
                {
                   { "url", new OpenApiString(logo) },
                   { "altText", new OpenApiString(logoAltText) },
                }
              },
            };
        }
    }
}

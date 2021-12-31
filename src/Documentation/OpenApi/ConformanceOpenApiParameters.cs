using Microsoft.OpenApi.Models;

namespace Documentation.OpenApi
{
    public static class ConformanceOpenApiParameters
    {
        public static readonly OpenApiParameter Id = new()
        {
            Name = "id",
            In = ParameterLocation.Path,
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
            },
        };

        public static readonly OpenApiParameter Body = new()
        {
            Name = "body",
            In = ParameterLocation.Query,
            Schema = new OpenApiSchema
            {
                Type = "string",
            },
        };

        public static readonly OpenApiParameter Format = new()
        {
            Name = "_format",
            In = ParameterLocation.Query,
            Description = "Format parameter can use to get response by setting _format param value from xml by _format=xml and response from json by _format=json",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
            },
        };

        public static readonly OpenApiParameter Count = new()
        {
            Name = "_count",
            In = ParameterLocation.Query,
            Schema = new OpenApiSchema
            {
                Type = "string",
            },
        };

        public static readonly OpenApiParameter Since = new()
        {
            Name = "_since",
            In = ParameterLocation.Query,
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
            },
        };

        public static readonly OpenApiParameter VId = new()
        {
            In = ParameterLocation.Path,
            Name = "vid",
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
            },
        };

        public static OpenApiParameter Convert(dynamic param)
        {
            return new OpenApiParameter
            {
                In = ParameterLocation.Query,
                Name = param.Name,
                Description = param.Documentation.Value,
                Schema = new OpenApiSchema
                {
                    Type = ConformanceOpenApiSchema.GetOpenApiSchemaType(param.Type.ToString()),
                    Format = ConformanceOpenApiSchema.GetOpenApiSchemaFormat(param.Type.ToString()),
                },
            };
        }
    }
}

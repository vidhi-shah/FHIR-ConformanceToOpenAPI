using System.Linq;

namespace Documentation.OpenApi
{
    public static class ConformanceOpenApiSchema
    {
        public static readonly string[] Types = { "integer", "number", "string", "boolean", "array", "object" };

        public static string GetOpenApiSchemaType(string type)
        {
            if (Types.Contains(type))
            {
                return type;
            }

            if (type == "quantity")
            {
                return "integer";
            }

            return "string";
        }

        public static string GetOpenApiSchemaFormat(string type)
        {
            return type == "date" ? "date" : string.Empty;
        }
    }
}
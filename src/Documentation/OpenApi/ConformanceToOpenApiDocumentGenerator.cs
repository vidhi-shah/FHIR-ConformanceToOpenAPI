using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;

namespace Documentation.OpenApi
{
    public class ConformanceToOpenApiDocumentGenerator : IOpenApiDocumentGenerator
    {
        private static readonly string[] _instanceInteractions = { "Read", "Vread", "Update", "Delete", "History" };
        private static readonly string[] _typeInteractions = { "Create", "SearchType", "HistoryType", "Delete", "HistoryInstance" };
        private static readonly string[] _allInteractions = _instanceInteractions.Concat(_typeInteractions).ToArray();
        private static string _authUrl;
        private static string _tokenUrl;
        public ConformanceToOpenApiDocumentGenerator(string authUrl, string tokenUrl)
        {
            _authUrl = authUrl;
            _tokenUrl = tokenUrl;
        }

        public async Task<OpenApiDocument> GenerateAsync(string hostUrl)
        {
            var openApiDoc = new OpenApiDocument();
            openApiDoc.Servers = new[]
            {
                new OpenApiServer { Url = hostUrl },
            };
            openApiDoc.Paths = new OpenApiPaths();
            openApiDoc.Info = new OpenApiInfo();

            var resources = await Helper.GetResourcesFromMetadata(new System.Uri(hostUrl));

            var securityScheme = GetDefaultSecurityScheme(GetDefaultScopes(resources));
            openApiDoc.Components = new OpenApiComponents();
            openApiDoc.Components.SecuritySchemes.Add(securityScheme.Name, securityScheme);

            openApiDoc.Components.Parameters = new Dictionary<string, OpenApiParameter>() { { "formatParam", ConformanceOpenApiParameters.Format } };

            foreach (var resource in resources)
            {
                if (resource.SearchParam.Count > 0)
                {
                    openApiDoc.Tags.Add(new OpenApiTag() { Name = resource.Type.ToString() });

                    var openApiPathItem = new OpenApiPathItem();
                    var typeOps = "/" + resource.Type.ToString();
                    var instOps = typeOps + "/{id}";

                    var openApiMediaType = new OpenApiMediaType()
                    {
                        Schema = new OpenApiSchema()
                        {
                            Type = "array",
                            Items = new OpenApiSchema() { Reference = new OpenApiReference() { Type = ReferenceType.Schema, Id = "components/schemas/", ExternalResource = resource.Type.ToString() } },
                        },
                    };

                    var response200 = new OpenApiResponse()
                    {
                        Description = "Bundle of " + resource.Type.ToString() + " resources",
                        Content = new Dictionary<string, OpenApiMediaType>() { { "application/json", openApiMediaType } },
                    };

                    var interactions = _allInteractions;
                    if (resource.Interaction.Count > 0)
                    {
                        interactions = resource.Interaction.Select(s => s.Code.ToString()).ToArray();
                    }

                    if (interactions.Contains("Create"))
                    {
                        openApiPathItem.AddOperation(OperationType.Post, CreateOperation("POST", resource.Type.ToString(), response200, ConformanceOpenApiParameters.Body));
                    }

                    if (interactions.Contains("SearchType"))
                    {
                        var parameters = new List<OpenApiParameter>
                    {
                        ConformanceOpenApiParameters.Format,
                    };

                        foreach (var param in resource.SearchParam)
                        {
                            parameters.Add(ConformanceOpenApiParameters.Convert(param));
                        }

                        openApiPathItem.AddOperation(OperationType.Get, CreateOperation("GET", resource.Type.ToString(), response200, parameters.ToArray()));
                    }

                    openApiDoc.Paths.Add("/" + resource.Type.ToString(), openApiPathItem);

                    openApiPathItem = new OpenApiPathItem();

                    if (interactions.Contains("Read"))
                    {
                        openApiPathItem.AddOperation(OperationType.Get, CreateOperation("GET /{id}", resource.Type.ToString(), response200));
                    }

                    if (interactions.Contains("Update"))
                    {
                        openApiPathItem.AddOperation(OperationType.Put, CreateOperation("Update", resource.Type.ToString(), response200, ConformanceOpenApiParameters.Body));
                    }

                    if (interactions.Contains("Delete"))
                    {
                        openApiPathItem.AddOperation(OperationType.Delete, CreateOperation("Delete", resource.Type.ToString(), response200));
                    }

                    openApiDoc.Paths.Add("/" + resource.Type.ToString() + "/{id}", openApiPathItem);

                    if (interactions.Contains("HistoryInstance"))
                    {
                        openApiPathItem = new OpenApiPathItem();
                        openApiPathItem.AddOperation(OperationType.Get, CreateOperation("History Get", resource.Type.ToString(), response200, ConformanceOpenApiParameters.Id, ConformanceOpenApiParameters.Count, ConformanceOpenApiParameters.Since));
                        openApiDoc.Paths.Add("/" + resource.Type.ToString() + "/{id}/_history", openApiPathItem);
                    }

                    if (interactions.Contains("HistoryType"))
                    {
                        openApiPathItem = new OpenApiPathItem();
                        openApiPathItem.AddOperation(OperationType.Get, CreateOperation("History Type Get", resource.Type.ToString(), response200, ConformanceOpenApiParameters.Count, ConformanceOpenApiParameters.Since));
                        openApiDoc.Paths.Add("/" + resource.Type.ToString() + "/_history", openApiPathItem);
                    }

                    if (interactions.Contains("Vread"))
                    {
                        openApiPathItem = new OpenApiPathItem();
                        openApiPathItem.AddOperation(OperationType.Get, CreateOperation("Version Read", resource.Type.ToString(), response200, ConformanceOpenApiParameters.Id, ConformanceOpenApiParameters.VId));
                        openApiDoc.Paths.Add("/" + resource.Type.ToString() + "/{id}/_history/{vid}", openApiPathItem);
                    }
                }
            }

            return openApiDoc;
        }

        private static Dictionary<string, string> GetDefaultScopes(List<Hl7.Fhir.Model.CapabilityStatement.ResourceComponent> resources)
        {
            var scopeList = new Dictionary<string, string>();
            foreach (var res in resources)
            {
                var read = "patient/" + res.Type.ToString() + "/read";
                var write = "patient/" + res.Type.ToString() + "/write";
                var all = "patient/" + res.Type.ToString() + "/all";
                scopeList.Add(read, read);
                scopeList.Add(write, write);
                scopeList.Add(all, all);
            }

            return scopeList;
        }

        private static Dictionary<string, string> GetResourceScopes(string resourceType, string action)
        {
            var scopeList = new Dictionary<string, string>();
            var read = "patient/" + resourceType + "/read";
            var write = "patient/" + resourceType + "/write";
            var all = "patient/" + resourceType + "/all";
            switch (action)
            {
                case "POST":
                case "PUT":
                    scopeList.Add(read, read);
                    scopeList.Add(write, write);
                    break;
                case "GET":
                    scopeList.Add(read, read);
                    break;
                case "DELETE":
                    scopeList.Add(read, read);
                    scopeList.Add(write, write);
                    scopeList.Add(all, all);
                    break;
            }

            return scopeList;
        }

        private static OpenApiOperation CreateOperation(string summary, string resourceType, OpenApiResponse response200, params OpenApiParameter[] parameters)
        {
            var operation = new OpenApiOperation();
            operation.Summary = summary;
            operation.Tags.Add(new OpenApiTag() { Name = resourceType });

            operation.Responses.Add("200", response200);

            foreach (var p in parameters)
            {
                operation.Parameters.Add(p);
            }

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                { GetDefaultSecurityScheme(GetResourceScopes(resourceType, summary)), System.Array.Empty<string>() },
            });

            return operation;
        }

        private static OpenApiSecurityScheme GetDefaultSecurityScheme(Dictionary<string, string> scopes)
        {
            var openApiScheme = new OpenApiSecurityScheme();
            openApiScheme.In = ParameterLocation.Header;
            openApiScheme.Name = "oAuthSample";
            openApiScheme.OpenIdConnectUrl = new System.Uri(_authUrl);
            openApiScheme.Scheme = SecuritySchemeType.OAuth2.ToString();
            openApiScheme.Type = SecuritySchemeType.OAuth2;
            openApiScheme.Flows = new OpenApiOAuthFlows()
            {
                AuthorizationCode = new OpenApiOAuthFlow()
                {
                    AuthorizationUrl = new System.Uri(_authUrl),
                    TokenUrl = new System.Uri(_tokenUrl),
                    Scopes = scopes,
                },
            };

            return openApiScheme;
        }
    }
}

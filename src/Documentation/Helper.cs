using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace Documentation
{
    public static class Helper
    {
        public static async Task<List<CapabilityStatement.ResourceComponent>> GetResourcesFromMetadata(Uri fhirMetadataUrl)
        {
            using (var client = new FhirClient(fhirMetadataUrl))
            {
                var capabilityStatement = await client.CapabilityStatementAsync();
                return capabilityStatement.Rest?.FirstOrDefault().Resource;
            }
        }
    }
}

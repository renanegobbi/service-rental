using Microsoft.OpenApi.Models;
using Rental.Api.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Concurrent;
using System;
using System.Linq;

namespace Rental.Api.Filters
{
    public class RemoveUnmarkedSchemasFilter : IDocumentFilter
    {
        public class SchemaTypeRegistryFilter : ISchemaFilter
        {
            public static ConcurrentDictionary<string, Type> SchemaTypes = new();

            public void Apply(OpenApiSchema schema, SchemaFilterContext context)
            {
                var schemaId = context.Type.Name;

                SchemaTypes[schemaId] = context.Type;
            }
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var toRemove = swaggerDoc.Components.Schemas
                .Where(kvp =>
                {
                    if (SchemaTypeRegistryFilter.SchemaTypes.TryGetValue(kvp.Key, out var type))
                    {
                        return !typeof(IExposeInSwagger).IsAssignableFrom(type);
                    }
                    return true;
                })
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in toRemove)
            {
                swaggerDoc.Components.Schemas.Remove(key);
            }
        }
    }
}

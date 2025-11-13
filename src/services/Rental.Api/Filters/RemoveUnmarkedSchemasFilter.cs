using Microsoft.OpenApi.Models;
using Rental.Api.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
            var referencedSchemas = new HashSet<string>(StringComparer.Ordinal);

            void CollectSchemaRefs(OpenApiSchema schema)
            {
                if (schema == null)
                    return;

                if (schema.Reference?.Id != null)
                    referencedSchemas.Add(schema.Reference.Id);

                if (schema.Items != null)
                    CollectSchemaRefs(schema.Items);

                if (schema.Properties != null)
                {
                    foreach (var prop in schema.Properties.Values)
                        CollectSchemaRefs(prop);
                }

                if (schema.AllOf != null)
                {
                    foreach (var s in schema.AllOf)
                        CollectSchemaRefs(s);
                }

                if (schema.OneOf != null)
                {
                    foreach (var s in schema.OneOf)
                        CollectSchemaRefs(s);
                }

                if (schema.AnyOf != null)
                {
                    foreach (var s in schema.AnyOf)
                        CollectSchemaRefs(s);
                }
            }

            foreach (var pathItem in swaggerDoc.Paths.Values)
            {
                foreach (var op in pathItem.Operations.Values)
                {
                    if (op.RequestBody != null)
                    {
                        foreach (var content in op.RequestBody.Content.Values)
                            CollectSchemaRefs(content.Schema);
                    }

                    foreach (var response in op.Responses.Values)
                    {
                        foreach (var content in response.Content.Values)
                            CollectSchemaRefs(content.Schema);
                    }
                }
            }

            var toRemove = swaggerDoc.Components.Schemas
                .Where(kvp =>
                {
                    var schemaName = kvp.Key;

                    if (referencedSchemas.Contains(schemaName))
                        return false;

                    if (!SchemaTypeRegistryFilter.SchemaTypes.TryGetValue(schemaName, out var type))
                        return false;

                    return !typeof(IExposeInSwagger).IsAssignableFrom(type);
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


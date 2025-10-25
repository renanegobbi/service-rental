using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rental.Api.Configuration
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.ExampleFilters();

                c.AddServer(new OpenApiServer
                {
                    Url = "/api/rentalservice",
                    Description = "Base path da API Rental Service"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter the JWT token like this: Bearer {your token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] {}
                            }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();

            });
        }

        public static class SwaggerRuntimeInfo
        {
            public static string? CurrentBaseUrl { get; set; }
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, IWebHostEnvironment webHostEnvironment)
        {
            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    var baseUrl = $"{httpReq.Scheme}://{httpReq.Host.Value}{httpReq.PathBase}/api/rentalservice";

                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = baseUrl }
                    };

                    if (!string.IsNullOrEmpty(swaggerDoc.Info.Description))
                    {
                        swaggerDoc.Info.Description = swaggerDoc.Info.Description
                            .Replace("{BASE_URL}", baseUrl);
                    }
                });

                options.RouteTemplate = "api/rentalservice/docs/{documentName}/swagger.json";

            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api/rentalservice/docs";
                c.DocumentTitle = "API Rental - Documentation";
                c.DocExpansion(DocExpansion.None);
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/api/rentalservice/docs/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                c.InjectStylesheet("/swagger-ui/custom.css");
                c.InjectStylesheet($"/swagger-ui/custom-{webHostEnvironment.EnvironmentName}.css");
                c.IndexStream = () => new GetStreamManifestResource().ManifestResourceStream();
                c.InjectJavascript("/js/swagger/custom.js");
                c.EnableValidator(null);
            });
        }

        public class GetStreamManifestResource
        {
            public Stream ManifestResourceStream() => GetType().GetTypeInfo().GetTypeInfo()
                .Assembly.GetManifestResourceStream("Rental.Api.Swagger.UI.Index.html");
        }

        public static class EnvironmentNames
        {
            public const string Development = "Development";
            public const string DbDevelopmentName = "RentalServiceDb";

            public const string Staging = "Staging";
            public const string DbStagingName = "HRentalServiceDb";

            public const string Production = "Production";
            public const string DbProductionName = "PRentalServiceDb";
        }

        public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
        {
            readonly IApiVersionDescriptionProvider provider;

            readonly IWebHostEnvironment webHostEnvironment;

            private readonly IConfiguration config;

            public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IWebHostEnvironment webHostEnvironment, IConfiguration config)
            {
                this.provider = provider;
                this.webHostEnvironment = webHostEnvironment;
                this.config = config;
            }

            public void Configure(SwaggerGenOptions options)
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, webHostEnvironment, config));
                }
            }

            static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description,
               IWebHostEnvironment webHostEnvironment, IConfiguration config)
            {
                var nomeBancoDados = string.Empty;

                var environmentName = webHostEnvironment.EnvironmentName;

                switch (environmentName)
                {
                    case EnvironmentNames.Development:
                        nomeBancoDados = EnvironmentNames.DbDevelopmentName;
                        break;
                    case EnvironmentNames.Staging:
                        nomeBancoDados = EnvironmentNames.DbStagingName;
                        break;
                    case EnvironmentNames.Production:
                        nomeBancoDados = EnvironmentNames.DbProductionName;
                        break;
                }

                var runtimeBaseUrl = SwaggerRuntimeInfo.CurrentBaseUrl ?? "https://localhost:7001/api/rentalservice";
                var basePath = "/api/rentalservice";

                var customDescription = "This API manages motorcycle rental operations, including registration, availability, booking, and customer interactions. It is designed for mobility platforms and rental services.<br>" +
                                    "<ul>" +
                                        $"<li>Current environment: <b>{environmentName.ToUpper()}</b></li>" +
                                        $"<li>Database (PostgreSQL): <b>{nomeBancoDados}</b></li>" +
                                        "<li>Base URL: <b>{BASE_URL}</b></li>" +
                                        $"<li>Use the 'Authorize' button above to authenticate using a JWT token.</li>" +
                                    "</ul>";

                customDescription += "<p>Contact information:</p>" +
                           "<ul>" +
                                "<li>E-mail: <b>renan@email.com</b></li>" +
                           "</ul>";

                var info = new OpenApiInfo()
                {
                    Title = "Rental API",
                    Version = description.ApiVersion.ToString(),
                    Description = customDescription,
                };

                if (description.IsDeprecated)
                {
                    info.Description += " This version is obsolete!";
                }

                return info;
            }
        }

        public class SwaggerDefaultValues : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                if (operation.Parameters == null)
                {
                    return;
                }

                foreach (var parameter in operation.Parameters)
                {
                    var description = context.ApiDescription
                        .ParameterDescriptions
                        .First(p => p.Name == parameter.Name);

                    var routeInfo = description.RouteInfo;

                    operation.Deprecated = OpenApiOperation.DeprecatedDefault;

                    if (parameter.Description == null)
                    {
                        parameter.Description = description.ModelMetadata?.Description;
                    }

                    if (routeInfo == null)
                    {
                        continue;
                    }

                    if (parameter.In != ParameterLocation.Path && parameter.Schema.Default == null)
                    {
                        parameter.Schema.Default = new OpenApiString(routeInfo.DefaultValue.ToString());
                    }

                    parameter.Required |= !routeInfo.IsOptional;
                }
            }
        }
    }
}
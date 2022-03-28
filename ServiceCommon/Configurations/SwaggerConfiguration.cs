using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CRM.ServiceCommon.Configurations
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfigureSwaggerOptions" /> class.
        /// </summary>
        /// <param name="provider">
        ///     The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger
        ///     documents.
        /// </param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this.provider = provider;
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "Sample API",
                Version = description.ApiVersion.ToString(),
                Description = "A sample application with Swagger, Swashbuckle, and API versioning."
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>
        ///     Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                parameter.Description ??= description.ModelMetadata?.Description;

                if (parameter.Schema.Default == null && description.DefaultValue != null)
                {
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.GetParameters().All(p => p.ParameterType != typeof(IFormFile)))
            {
                return;
            }

            var uploadFileMediaType = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties =
                    {
                        ["file"] = new OpenApiSchema
                        {
                            Description = "Upload File",
                            Type = "file",
                            Format = "binary"
                        }
                    },
                    Required = new HashSet<string>
                    {
                        "file"
                    }
                }
            };
            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["multipart/form-data"] = uploadFileMediaType
                }
            };
        }
    }

    public static class SwaggerConfiguration
    {
        public static void ConfigureSwagger(this IServiceCollection services, string xmlPath)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();
                    options.OperationFilter<FileUploadOperationFilter>();

                    options.IncludeXmlComments(xmlPath);

                    options.MapType<FileResult>(() => new OpenApiSchema
                    {
                        Type = "file"
                    });
                });
        }
    }
}
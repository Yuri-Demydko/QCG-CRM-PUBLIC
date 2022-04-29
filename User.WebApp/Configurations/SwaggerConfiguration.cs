using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CRM.IdentityServer.Extensions.Constants;
using CRM.ServiceCommon.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OData.Swagger.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CRM.User.WebApp.Configurations
{
    internal static class SecuritySchemes
    {
        public static OpenApiSecurityScheme OAuthScheme => new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            },
            Scheme = "oauth2",
            Name = JwtBearerDefaults.AuthenticationScheme,
            In = ParameterLocation.Header
        };

        public static OpenApiSecurityScheme BearerScheme(IConfiguration configuration)
        {
            return new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Description = "Standard authorisation using the Bearer scheme. Example: \"bearer {token}\"",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                OpenIdConnectUrl =
                    new Uri(
                        $"{configuration.GetSection("IdentityServerClient:AuthorizationServiceUrl").Value}/.well-known/openid-configuration"),
                BearerFormat = "JWT",
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl =
                            new Uri(
                                $"{configuration.GetSection("IdentityServerClient:AuthorizationServiceUrl").Value}/connect/authorize"),
                        Scopes = new Dictionary<string, string>
                        {
                            { Scopes.Roles, Scopes.Roles }
                        },
                        TokenUrl = new Uri(
                            $"{configuration.GetSection("IdentityServerClient:AuthorizationServiceUrl").Value}/connect/token")
                    }
                }
            };
        }
    }


    public static class SwaggerConfiguration
    {
        public static IServiceCollection ConfigureSwaggerBearer(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                {
                    // options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    //     SecuritySchemes.BearerScheme(configuration));
                    //
                    // options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    // {
                    //     { SecuritySchemes.OAuthScheme, new List<string>() }
                    // });
                    
                    var securityScheme = new OpenApiSecurityScheme()
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT" // Optional
                    };

                    var securityRequirement = new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "bearerAuth"
                                }
                            },
                            new string[] {}
                        }
                    };

                    options.AddSecurityDefinition("bearerAuth", securityScheme);
                    options.AddSecurityRequirement(securityRequirement);

                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();
                    options.OperationFilter<FileUploadOperationFilter>();

                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                });
           services.AddOdataSwaggerSupport();
            return services;
        }
    }
}
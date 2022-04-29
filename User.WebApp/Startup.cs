using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using ClosedXML.Excel;
using CRM.DAL.Models.DatabaseModels.Users;
using CRM.IdentityServer.Extensions.Constants;
using CRM.ServiceCommon.Configurations;
using CRM.ServiceCommon.Middlewares;
using CRM.User.WebApp.Configurations;
using CRM.User.WebApp.Models.Basic;
using CRM.User.WebApp.Services;
using CRM.User.WebApp.Services.PayCardValidationService;
using CRM.User.WebApp.Services.ProductBuy;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Microsoft.OData.ODataUrlKeyDelimiter;
using ClaimTypes = System.Security.Claims.ClaimTypes;

namespace CRM.User.WebApp
{
    /// <summary>
    ///     Represents the startup process for the application.
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }


        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Env { get; }

        /// <summary>
        ///     Configures services for the application.
        /// </summary>
        /// <param name="services">The collection of services to configure the application with.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureIdentityServerClient(Configuration);

            services.Configure<IdentityOptions>(options =>
                {
                    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
                }
                );

            services.AddIdentityForWebApi<DAL.Models.DatabaseModels.Users.User, Role>(options =>
                {
                    options.ClaimsIdentity.UserIdClaimType = IdentityServer.Extensions.Constants.ClaimTypes.UserId;

                })
                .AddEntityFrameworkStores<UserDbContext>()
                .AddRoles<Role>()
                .AddDefaultTokenProviders();

            services.Configure<SecurityStampValidatorOptions>(options =>
                options.ValidationInterval = TimeSpan.FromSeconds(10));

            //???
            if (!Env.IsDevelopment())
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders =
                        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                });
            }

            services.AddCors();
            
            // the sample application always uses the latest version, but you may want an explicit version such as Version_2_2
            // note: Endpoint Routing is enabled by default; however, it is unsupported by OData and MUST be false
            services.AddControllers(options =>
                {
                    options.EnableEndpointRouting = false;
                })
                .AddNewtonsoftJson()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                .AddFluentValidation(s => { s.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            services.AddApiVersioning(options => options.ReportApiVersions = true);
            services.AddOData().EnableApiVersioning();
            services.AddControllersWithViews();
            
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "React-app"; });

            services.AddODataApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            // services.ConfigureSwagger(xmlPath);
            services.ConfigureSwaggerBearer(Configuration);
            

            services.AddHttpContextAccessor();

            services.ConfigureDatabase(Configuration);

            services.ConfigureSqlKata(Configuration);

            services.AddDataProtection(options =>
                    options.ApplicationDiscriminator = "User Web App"
                )
                .PersistKeysToDbContext<UserDbContext>();

            services.ConfigureEmail(Configuration);
            
             services.ConfigureEmailCodes(Configuration);

            services.ConfigureRazorTemplateEngine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            services.AddHangfire(config =>
            {
                config.UseNLogLogProvider();

                if (!Env.IsDevelopment())
                {
                    
                    //@TODO: CREATE SCHEMA FOR PROD MODE
                    var connectionString = Configuration.GetConnectionString("CRM");
                    config.UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions()
                    {
                        SchemaName = "hangfire-crm"
                    });
                }
                else
                {
                    config.UseInMemoryStorage();
                }
            });
            
            services.AddSignalR().AddNewtonsoftJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    options.PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.PayloadSerializerSettings.Converters.Add(new StringEnumConverter());
                }
            );
            
            services.ConfigureDbEventHostedService(Configuration);

            if(Env.IsDevelopment())
            {
                services.AddScoped<IProductBuyService, ProductBuyServiceMock>();
                services.AddScoped<IPayCardValidationService, PayCardValidationServiceMock>();
            }
            
            services.ConfigureAutoMapper();

            services.AddAutoMapper(config =>
            {
       
            }, typeof(Startup));
        }

        /// <summary>
        ///     Configures the application using the provided builder, hosting environment, and logging factory.
        /// </summary>
        /// <param name="app">The current application builder.</param>
        /// <param name="modelBuilder">
        ///     The <see cref="VersionedODataModelBuilder">model builder</see> used to create OData entity
        ///     data models (EDMs).
        /// </param>
        /// <param name="provider">The API version descriptor provider used to enumerate defined API versions.</param>
        /// <param name="env">
        ///     Provides the information about the web hosting environment an application is running in.
        /// </param>
        public void Configure(
            IApplicationBuilder app,
            VersionedODataModelBuilder modelBuilder,
            IApiVersionDescriptionProvider provider)
        {
            if (!Env.IsDevelopment())
            {
                app.UseHsts();
                app.UseForwardedHeaders();
            }
            
            if (!Env.IsProduction())
            {
               app.UseDeveloperExceptionPage(); 
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax, Secure = CookieSecurePolicy.None });

            app.UseCors(b=>b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();

            if (Env.IsProduction())
            {
                app.UseMiddleware<SwaggerMiddleware>();
            }
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new List<IDashboardAuthorizationFilter>()
                }).RequireAuthorization(new AuthorizeAttribute
                {
                    Roles = UserRoles.Admin
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                });
            

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                Queues = new[] {"digital", "user", "default"},
                WorkerCount = 30
            });
            

            app.UseMvc(
                routeBuilder =>
                {
                    routeBuilder.SetTimeZoneInfo(TimeZoneInfo.Local);

                    routeBuilder.ServiceProvider.GetRequiredService<ODataOptions>().UrlKeyDelimiter = Parentheses;

                    // global odata query options
                    routeBuilder.Count().MaxTop(1000).Select().Expand().Filter();

                    routeBuilder.MapVersionedODataRoutes("odata", "api/v{v:apiVersion}", modelBuilder.GetEdmModels());
                });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "React-app";

                if (Env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
            
        }
    }
}
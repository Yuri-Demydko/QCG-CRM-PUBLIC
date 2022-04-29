using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using AspNetCore.ReCaptcha;
using CRM.IdentityServer.Models;
using CRM.IdentityServer.Configuration;
using CRM.IdentityServer.Services;
using CRM.ServiceCommon.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.DataProtection;
using Hangfire;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace CRM.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "An ASP.NET Core Web API for managing ToDo items",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });
            });
            
            services.AddHealthChecks();

            services.ConfigureDatabase(Configuration);
            services.ConfigureIdentityServer(Configuration);

            services.AddApiVersioning(options => options.ReportApiVersions = true);

            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            services.ConfigureEmail(Configuration);
            services.ConfigureRazorTemplateEngine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            
            services.ConfigureEmailCodes(Configuration);

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
            

            services.AddDataProtection(options => options.ApplicationDiscriminator = "Identity server"
                )
                .PersistKeysToDbContext<IdentityServerDbContext>();
            
            
            services.AddReCaptcha(Configuration.GetSection("ReCaptcha"));

            services.AddHangfire(config => config.UseInMemoryStorage());
            
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IApiVersionDescriptionProvider provider)
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

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax, Secure = CookieSecurePolicy.None });

            app.UseCors(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseIdentityServer();

            app.UseRouting();

            app.UseHangfireServer(options: new BackgroundJobServerOptions()
            {
                Queues = new[] {"identity"},
                ServerName = "Identity"
            });
            
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllerRoute(
                    name: "default",
                     pattern: "{controller=Account}/{action=Login}/{id?}");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHangfireDashboard();
            });
            
            if (Env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    options.RoutePrefix = "swagger";
                });
            }
            
            RecurringJob.AddOrUpdate<AccountsService>(j => j.CleanUnconfirmedAccounts(), Cron.Daily());
        }
    }
}
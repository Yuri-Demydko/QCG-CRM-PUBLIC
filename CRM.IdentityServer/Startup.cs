using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using AspNetCore.ReCaptcha;
using CRM.IdentityServer.Models;
using CRM.IdentityServer.Services;
using CRM.IdentityServer.Configuration;
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

            services.AddHealthChecks();

            services.ConfigureDatabase(Configuration);
            services.ConfigureIdentityServer(Configuration);

            services.AddApiVersioning(options => options.ReportApiVersions = true);
            
            
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }
    }
}
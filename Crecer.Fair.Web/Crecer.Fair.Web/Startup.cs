using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Crecer.Fair.Web.Models;
using Ecubytes.AspNetCore.DataProtection;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Ecubytes.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace Crecer.Fair.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGlobalOptions(this.Configuration);
            services.AddDefaultHttpHandlers(Configuration);                                           

            services.AddDefaultLocalization(op => op.ResourcesPath = "Resources");

            services.AddControllersWithViews().
                AddRazorRuntimeCompilation().
                AddDefaultJsonOptions(options =>
                {
                    options.UseStringEnumConverter = true;
                });       

            services.Configure<SmtpSettingsOptions>(
                this.Configuration.GetSection(SmtpSettingsOptions.SectionName));     

            //services.AddDataProtectionSharedCookies(Configuration);

             DataProtectionSharedCookiesOptions soptions = Configuration.GetSection(DataProtectionSharedCookiesOptions.SectionName).
                Get<DataProtectionSharedCookiesOptions>();

            services.AddDataProtection()
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(Environment.ExpandEnvironmentVariables(soptions.PersistKeysPath)))
                //.PersistKeysToFileSystem(new System.IO.DirectoryInfo(soptions.PersistKeysPath))
                .SetApplicationName(soptions.SharedApplicationName);
                
            services.AddDefaultAuthenticationService(Configuration);
            services.AddDefaultAuthorizationPolicyProvider();
            services.AddHttpClientServices(Configuration);

            services.AddWebOptimizer(pipeline =>
            {
                pipeline.CompileScssFiles();
                pipeline.AddFiles("text/css", 
                    "/css/custom.css",
                    "/css/fair-model/stand-1.css",
                    "/css/fair-model/stand-2.css",
                    "/css/fair-model/stand-3.css",
                    "/css/fair-model/stand-4.css",
                    "/css/fair-model/stand-5.css",
                    "/css/fair-model/stand-6.css",
                    "/css/fair-model/stand-7.css",
                    "/css/fair-model/stand-8.css",
                    "/css/fair-model/stand-9.css",
                    "/css/fair-model/stand-10.css",
                    "/css/fair-model/stand-11.css",
                    "/css/fair-model/stand-12.css"
                );
                
                pipeline.AddScssBundle("/css/style.default.css","/css/style.default.scss");                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Global static service
            ServiceActivator.Configure(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseWebOptimizer();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseUserLastAccessUpdate(options => {
                options.ApiProfileName = "Identity.User";                
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

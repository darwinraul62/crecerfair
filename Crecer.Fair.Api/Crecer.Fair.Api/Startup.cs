using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Crecer.Fair.Api.Authorization.HostedServices;
using Crecer.Fair.Api.Infrastructure.AutofacModules;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Ecubytes.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Crecer.Fair.Api
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

            services.AddAutoMapper(typeof(Startup));            
            
            services.AddDefaultMvcSettings();

            services.AddAuthorizationServer(Configuration);

            services.AddHostedService<AuthorizationSettingsWorker>();

            services.AddDefaultHttpHandlers(Configuration);

            // DataProtection configuration is added
            // Set the directory where the keyring will be stored
            // to be shared by other applications that have in common
            // the application name that is set below
            // Required for Cookies shared           
            services.AddDataProtectionSharedCookies(Configuration);
            
            services.AddControllers()
                .AddDefaultJsonOptions();

            services.AddDefaultAuthenticationService(Configuration);

            services
                .AddDefaultAuthorizationPolicyProvider()
                .AddFairDbContext(Configuration)
                .AddDefaultLocalization(Configuration);
                //.AddDefaultEventBusService(Configuration);       

            //services.AddMassTransitEventBus(Configuration);

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = ApiVersionReader.Combine(
                    //new HeaderApiVersionReader("api-version"),
                    new QueryStringApiVersionReader("version"));                    
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Crecer.Fair.Api", Version = "v1" });
            });
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterModule(new FairDataModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ServiceActivator.Configure(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Crecer.Fair.Api v1"));
            }

            app.UseDefaultLocalization();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

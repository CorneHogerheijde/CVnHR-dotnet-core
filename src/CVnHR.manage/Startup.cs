using CVnHR.Business.HrDataservice;
using CVnHR.Business.Kvk;
using CVnHR.Business.Kvk.Api;
using CVnHR.Business.Logging;
using CVnHR.Business.Services;
using CVnHR.manage.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CVnHR.manage
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            // TODO: clean this
            const string klantReferentie = "ACC_I_002"; // TODO set in config

            services.AddTransient<IHrDataServiceHttpClient, HrDataServiceHttpClient>();
            services.AddTransient<IHrDataservice, HrDataservice>((serviceProvider) => 
                new HrDataservice(klantReferentie, serviceProvider.GetService<IHrDataServiceHttpClient>()));
            services.AddTransient<LoggingHandler>();
            services.AddHttpClient("hrDataservice")
                .ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                    var hrDataService = serviceProvider.GetService<IHrDataServiceHttpClient>();
                    var handler = hrDataService.GetHttpClientHandler();
                    var certificate = hrDataService.GetCertificate();

                    // TODO: only install certificate after uploading??
                    hrDataService.InstallCertificate(certificate);

                    handler.ClientCertificates.Add(certificate);
                    return handler;
                })
                .AddHttpMessageHandler<LoggingHandler>();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IKvkSearchApi, KvkSearchApi>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}

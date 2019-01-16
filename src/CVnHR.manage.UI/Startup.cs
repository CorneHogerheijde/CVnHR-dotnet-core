using CVnHR.Business.HrDataservice;
using CVnHR.Business.Kvk;
using CVnHR.Business.Kvk.Api;
using CVnHR.Business.Logging;
using CVnHR.Business.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CVnHR.manage.UI
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
            // Add framework services.
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Simple example with dependency injection for a data provider.
            services.AddSingleton<Providers.IWeatherProvider, Providers.WeatherProviderFake>();

            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IKvkSearchApi, KvkSearchApi>();

            services.AddTransient<IHRDataserviceMessageParser, HRDataserviceMessageParser>();
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
                    handler.ClientCertificates.Add(certificate);
                    return handler;
                })
                .AddHttpMessageHandler<LoggingHandler>();

            services.AddDataProtection(); // TODO?
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Webpack initialization with hot-reload.
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}

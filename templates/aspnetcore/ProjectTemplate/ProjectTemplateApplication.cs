using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vostok.Commons.Extensions.UnitConvertions;
using Vostok.Hosting;
using Vostok.Instrumentation.AspNetCore;
using Vostok.Metrics;

namespace ProjectTemplate
{
    public class ProjectTemplateApplication : AspNetCoreVostokApplication
    {
        protected override void OnStarted(IVostokHostingEnvironment hostingEnvironment)
        {
            hostingEnvironment.MetricScope.SystemMetrics(1.Minutes());
        }

        protected override IWebHost BuildWebHost(IVostokHostingEnvironment hostingEnvironment)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://*:{hostingEnvironment.Configuration["port"]}/")
                .AddVostokServices()
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(s => s.AddMvc())
                .Configure(app =>
                {
                    var env = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();
                    app.UseVostok();
                    if (env.IsDevelopment())
                        app.UseDeveloperExceptionPage();
                    app.UseMvc();
                })
                .Build();
        }
    }
}
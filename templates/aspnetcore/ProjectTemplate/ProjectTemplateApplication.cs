using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Vostok.Commons.Extensions.UnitConvertions;
using Vostok.Hosting;
using Vostok.Instrumentation.AspNetCore;
using Vostok.Logging.Serilog;
using Vostok.Logging.Serilog.Enrichers;
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
            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.With<ThreadEnricher>()
                .Enrich.With<FlowContextEnricher>()
                .MinimumLevel.Debug()
                .WriteTo.Airlock();
            if (hostingEnvironment.Configuration.GetSection("logging").GetValue<bool>("console"))
                loggerConfiguration = loggerConfiguration.WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff} {Level:u3} [{Thread}] {Message:l}{NewLine}{Exception}", restrictedToMinimumLevel: LogEventLevel.Information);
            var logger = loggerConfiguration.CreateLogger();
            return new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:33333/")
                .AddVostokServices()
                .ConfigureServices(s => s.AddMvc())
                .UseSerilog(logger)
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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Vostok.Commons.Extensions.UnitConvertions;
using Vostok.Hosting;
using Vostok.Instrumentation.AspNetCore;
using Vostok.Logging.Serilog;
using Vostok.Logging.Serilog.Enrichers;
using Vostok.Metrics;
using Vostok.Tracing;

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
                .WriteTo.Airlock(LogEventLevel.Information);
            if (hostingEnvironment.Configuration.GetSection("logging").GetValue<bool>("console"))
                loggerConfiguration = loggerConfiguration.WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff} {Level:u3} [{Thread}] {Message:l}{NewLine}{Exception}", restrictedToMinimumLevel: LogEventLevel.Information);
            var logger = loggerConfiguration.CreateLogger();
            return new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:33333/")
                .AddVostokServices()
                .UseSerilog(logger)
                .Configure(app =>
                {
                    app.UseVostok();
                    app.Use(next => (async httpContext =>
                    {
                        httpContext.Response.StatusCode = 200;
                        using (var sw = new StreamWriter(httpContext.Response.Body))
                        {
                            sw.Write(JsonConvert.SerializeObject(new
                                {
                                    thisUrl = httpContext.Request.GetDisplayUrl(),
                                    traceUrl = $"http://localhost:6301/{TraceContext.Current.TraceId}",
                                }));
                        }
                    }));
                })
                .Build();
        }
    }
}
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Vstk.Commons.Extensions.UnitConvertions;
using Vstk.Hosting;
using Vstk.Instrumentation.AspNetCore;
using Vstk.Logging.Serilog;
using Vstk.Logging.Serilog.Enrichers;
using Vstk.Metrics;
using Vstk.Tracing;

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
            if (hostingEnvironment.Log != null)
                loggerConfiguration = loggerConfiguration.WriteTo.VostokLog(hostingEnvironment.Log);
            var logger = loggerConfiguration.CreateLogger();
            return new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://*:{hostingEnvironment.Configuration["port"]}/")
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
                            await sw.WriteAsync(JsonConvert.SerializeObject(new
                                {
                                    thisUrl = httpContext.Request.GetDisplayUrl(),
                                    traceUrl = $"http://localhost:6301/{TraceContext.Current.TraceId}",
                                    traceId = TraceContext.Current.TraceId
                                }));
                        }
                    }));
                })
                .Build();
        }
    }
}
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Vostok.Commons.Extensions.UnitConvertions;
using Vostok.Hosting;
using Vostok.Instrumentation.AspNetCore;
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
            return new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://*:{hostingEnvironment.Configuration["port"]}/")
                .AddVostokServices()
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddJsonFile("appsettings.json");
                })
                .Configure(app =>
                {
                    var env = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();
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
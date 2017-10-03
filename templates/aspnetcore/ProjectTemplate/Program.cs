using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Vostok.Instrumentation.AspNetCore;
using Vostok.Logging;
using Vostok.Logging.Serilog;

namespace ProjectTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithHost()
                .Enrich.WithProperty("ServiceName", Settings.ServiceName)
                .WriteTo.Async(x => x.RollingFile("log-{Date}.txt", outputTemplate: "{Timestamp:HH:mm:ss.fff} {Level} {Message:l} {Exception}{NewLine}{Properties}{NewLine}"))
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff} {Level} {Message:l} {Exception}{NewLine}{Properties}{NewLine}")
                .CreateLogger();
            var log = new SerilogLog(Log.Logger)
                .WithFlowContext();
            new WebHostBuilder()
                .UseKestrel()
                .UseUrls(Settings.ListenPrefix)
                .ConfigureLogging(builder => builder.AddVostok(log))
                .ConfigureServices(collection => collection.AddMvc())
                .Configure(app =>
                {
                    app.UseVostok(log);
                    app.UseDeveloperExceptionPage();
                    app.UseMvc();
                })
                .Build()
                .Run();
            Log.CloseAndFlush();
        }
    }
}

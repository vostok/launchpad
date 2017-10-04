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
            new WebHostBuilder()
                .UseKestrel()
                .UseUrls(Settings.ListenPrefix)
                .ConfigureLogging(logging =>
                {
                    const string outputTemplate =
                        "{Timestamp:HH:mm:ss.fff} {Level} {Message:l} {Exception}{NewLine}{Properties}{NewLine}";

                    Log.Logger = new LoggerConfiguration()
                        .Enrich.WithHost()
                        .Enrich.WithProperty("ServiceName", Settings.ServiceName)
                        .WriteTo.Async(x => x.RollingFile("log-{Date}.txt", outputTemplate: outputTemplate))
                        .WriteTo.Console(outputTemplate: outputTemplate)
                        .CreateLogger();

                    var log = new SerilogLog(Log.Logger)
                        .WithFlowContext();

                    logging.AddVostok(log);

                    logging.Services.AddSingleton(log);
                })
                .ConfigureServices(services =>
                {
                    services.AddMvc();
                })
                .Configure(app =>
                {
                    app.UseVostok();
                    app.UseDeveloperExceptionPage();
                    app.UseMvc();

                    var applicationLifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
                    applicationLifetime.ApplicationStopping.Register(Log.CloseAndFlush);
                })
                .Build()
                .Run();
        }
    }
}

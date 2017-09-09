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
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff} {Level} {Message:l} {Exception}{NewLine}{Properties}{NewLine}")
                .CreateLogger();
            var log = new SerilogLog(Log.Logger)
                .WithContext();
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
        }
    }
}

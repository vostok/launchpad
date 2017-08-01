using Serilog;
using Topshelf;
using Vostok.Logging;
using Vostok.Logging.Serilog;
using Vostok.Topshelf;

namespace ProjectTemplate
{
    internal static class Program
    {
        private static void Main()
        {
            HostFactory.Run(x =>
            {
                x.SetServiceName(Settings.ServiceName);
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff} {Level} {Message:l} {Exception}{NewLine}{Properties}{NewLine}")
                    .CreateLogger();
                var log = new SerilogLog(Log.Logger)
                    .WithContext();
                x.UseVostokLogging(log);
                x.Service(settings => new WebApiService(log));
            });
        }
    }
}
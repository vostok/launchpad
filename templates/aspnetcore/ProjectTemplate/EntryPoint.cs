using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Vostok.Hosting;
using Vostok.Logging;
using Vostok.Logging.Serilog;

namespace ProjectTemplate
{
    public static class EntryPoint
    {
        public static void Main(string[] args)
        {
            BuildVostokHost(args).Run();
        }

        private static IVostokHost BuildVostokHost(params string[] args)
        {
            return new VostokHostBuilder<ProjectTemplateApplication>()
                .SetServiceInfo("%project%", "%service%")
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddEnvironmentVariables();
                    configurationBuilder.AddJsonFile("hostsettings.json");
                    configurationBuilder.AddCommandLine(args);
                })
                .ConfigureHost((context, hostConfigurator) =>
                {
                    hostConfigurator.SetHostLog(CreateHostLog(context));
                })
                .Build();
        }

        private static ILog CreateHostLog(VostokHostBuilderContext context)
        {
            var configuration = context.Configuration.GetSection("hostLog");

            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .ConfigureConsoleLogging(configuration)
                .ConfigureFileLogging(configuration, context)
                .CreateLogger();

            return new SerilogLog(logger);
        }

        private static LoggerConfiguration ConfigureFileLogging(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, VostokHostBuilderContext context)
        {
            var logsDir = configuration["logsDir"];
            return string.IsNullOrEmpty(logsDir)
                ? loggerConfiguration
                : loggerConfiguration
                    .WriteTo.RollingFile(
                        Path.Combine(logsDir, context.HostingEnvironment.Service, "log-{Date}.log"),
                        outputTemplate: "{Timestamp:HH:mm:ss.fff} {Level:u3} [{Thread}] {Message:l}{NewLine}{Exception}");
        }

        private static LoggerConfiguration ConfigureConsoleLogging(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            return !configuration.GetValue<bool>("console")
                ? loggerConfiguration
                : loggerConfiguration
                    .WriteTo.Console(
                        outputTemplate: "{Timestamp:HH:mm:ss.fff} {Level:u3} [{Thread}] {Message:l}{NewLine}{Exception}",
                        restrictedToMinimumLevel: LogEventLevel.Information);
        }
    }
}
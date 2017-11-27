using Microsoft.Extensions.Configuration;
using Vostok.Hosting;
using Vostok.Logging;
using Vostok.Logging.Logs;

namespace ProjectTemplate
{
    public static class EntryPoint
    {
        public static void Main(string[] args)
        {
            BuildVostokHost(new ConsoleLog(), args).Run();
        }

        private static IVostokHost BuildVostokHost(ILog hostLog, params string[] args)
        {
            return new VostokHostBuilder<ProjectTemplateApplication>()
                .SetServiceInfo("%project%", "%service%")
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddCommandLine(args);
                    configurationBuilder.AddEnvironmentVariables();
                    configurationBuilder.AddJsonFile("appsettings.json");
                })
                .ConfigureHost(hostConfigurator => hostConfigurator.SetHostLog(hostLog))
                .Build();
        }
    }
}
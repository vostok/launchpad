using System.Web.Http;
using JetBrains.Annotations;
using Microsoft.Owin.Hosting;
using Owin;
using Topshelf;
using Vostok.Instrumentation.WebApi;
using Vostok.Logging;

namespace ProjectTemplate
{
    internal class WebApiService : ServiceControl
    {
        [NotNull] private readonly ILog log;

        public WebApiService([NotNull] ILog log)
        {
            this.log = log;
        }

        public bool Start([NotNull] HostControl hostControl)
        {
            WebApp.Start(Settings.ListenPrefix, app =>
            {
                app.UseVostokOwinLogging(log);
                var config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();

                app.UseVostok(log);
                app.UseWebApi(config);
            });
            log.Info($"Listening {Settings.ListenPrefix}");
            return true;
        }

        public bool Stop([NotNull] HostControl hostControl)
        {
            return true;
        }
    }
}
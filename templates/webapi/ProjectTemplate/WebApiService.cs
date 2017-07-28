using System.Web.Http;
using JetBrains.Annotations;
using Microsoft.Owin.Hosting;
using Owin;
using Topshelf;

namespace ProjectTemplate
{
    internal class WebApiService : ServiceControl
    {
        public bool Start([NotNull] HostControl hostControl)
        {
            WebApp.Start(Settings.ListenPrefix, builder =>
            {
                var config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();
                builder.UseWebApi(config);
            });
            return true;
        }

        public bool Stop([NotNull] HostControl hostControl)
        {
            return true;
        }
    }
}
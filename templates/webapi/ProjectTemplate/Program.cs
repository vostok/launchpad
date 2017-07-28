using Topshelf;

namespace ProjectTemplate
{
    internal static class Program
    {
        private static void Main()
        {
            HostFactory.Run(x =>
            {
                x.SetServiceName(Settings.ServiceName);
                x.Service<WebApiService>();
            });
        }
    }
}
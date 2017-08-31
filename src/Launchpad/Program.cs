using System;
using CommandLine;

namespace Launchpad
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Console.Error.WriteLine(e.ExceptionObject);
                Environment.Exit(-1);
            };
            Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    opts =>
                    {
                        new ProjectCopier(opts).Execute();
                        return 0;
                    },
                    errs => 1);
        }
    }
}
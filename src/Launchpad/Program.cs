using System;
using CommandLine;
using Launchpad.Create;

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

            Parser.Default.ParseArguments<CreateOptions>(args)
                .MapResult(
                    (CreateOptions opts) =>
                    {
                        new ProjectCopier(opts).Execute();
                        Console.Out.WriteLine($"Project {opts.ProjectName} created in '{opts.Output}' directory.");
                        return 0;
                    },
                    errs => 1);
        }
    }
}
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Launchpad.Create
{
    [Verb("create", HelpText = "Create boilerplate Vostok-instrumented C# project.")]
    internal class CreateOptions
    {
        [Option('n', "name", Required = true, HelpText = "Project name, must comply with C# namespace naming conventions.")]
        public string ProjectName { get; set; }

        [Option('o', "output", HelpText = "Output directory.", Default = "output")]
        public string Output { get; set; }

        [Option('t', "template", HelpText = "Template to use.", Default = "aspnetcore")]
        public string Template { get; set; }

        [Usage(ApplicationAlias = "vostok")]
        public static IEnumerable<Example> Examples
        {
            get { yield return new Example("Create ASP.NET Core 2 application named MyFirstProject in 'output' directory", new CreateOptions {ProjectName = "MyFirstProject"}); }
        }
    }
}
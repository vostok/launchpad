using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Launchpad.Create
{
    [Verb("create", HelpText = "Create boilerplate Vostok-instrumented C# project.")]
    internal class CreateOptions
    {
        [Option('p', "project", Required = true, HelpText = "Project name. Project is a group of services, a name of your company, or something like that.")]
        public string ProjectName { get; set; }

        [Option('s', "service", Required = true, HelpText = "Service name, must comply with C# namespace naming conventions.")]
        public string ServiceName { get; set; }

        [Option('o', "output", HelpText = "Output directory.", Default = "output")]
        public string Output { get; set; }

        [Option('t', "template", HelpText = "Template to use.", Default = "aspnetcore")]
        public string Template { get; set; }

        [Usage(ApplicationAlias = "vostok")]
        public static IEnumerable<Example> Examples
        {
            get { yield return new Example("Create ASP.NET Core 2 application named MyFirstService in 'output' directory", new CreateOptions {ProjectName = "MyFirstProject", ServiceName = "MyFirstService"}); }
        }
    }
}
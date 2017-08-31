using CommandLine;

namespace Launchpad
{
    internal class Options
    {
        [Option('n', "name", Required = true, HelpText = "Target project name")]
        public string ProjectName { get; set; }

        [Option('o', "output", HelpText = "Output directory", Default = ".\\output")]
        public string Output { get; set; }
    }
}
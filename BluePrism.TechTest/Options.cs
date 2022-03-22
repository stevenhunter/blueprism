using CommandLine;

namespace BluePrism.TechTest.Console
{
    public class Options
    {
        [Option('d', "dictionary", Required = true, HelpText = "Dictionary file name.")]
        public string Dictionary { get; set; }

        [Option('s', "startword", Required = true, HelpText = "Start word.")]
        public string StartWord { get; set; }

        [Option('e', "endword", Required = true, HelpText = "End word.")]
        public string EndWord { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file name.")]
        public string Output { get; set; }
    }
}

using CommandLine;

namespace BluePrism.TechTest.Console
{
    public class Options
    {
        [Option('d', "dictionary", Required = true, HelpText = "Dictionary file name.")]
        public string Dictionary { get; set; } = default!;

        [Option('s', "startword", Required = true, HelpText = "Start word.")]
        public string StartWord { get; set; } = default!;

        [Option('e', "endword", Required = true, HelpText = "End word.")]
        public string EndWord { get; set; } = default!;

        [Option('o', "output", Required = false, HelpText = "Output file name.")]
        public string Output { get; set; } = default!;
    }
}

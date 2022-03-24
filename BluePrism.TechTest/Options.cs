using CommandLine;

namespace BluePrism.TechTest.Console;

public class Options
{
    [Option('i', "inputFile", Required = true, HelpText = "Input dictionary file name.")]
    public string Dictionary { get; set; } = default!;

    [Option('s', "startWord", Required = true, HelpText = "Start word.")]
    public string StartWord { get; set; } = default!;

    [Option('e', "endWord", Required = true, HelpText = "End word.")]
    public string EndWord { get; set; } = default!;

    [Option('o', "outputFile", Required = false, HelpText = "Output file name.")]
    public string Output { get; set; } = default!;
}
// See https://aka.ms/new-console-template for more information
using BluePrism.TechTest.Library.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using CommandLine;
using BluePrism.TechTest.Console;
using BluePrism.TechTest.Library;

var parserResult = Parser.Default.ParseArguments<Options>(args);
await parserResult.WithParsedAsync(RunOptionsAsync);
parserResult.WithNotParsed(HandleParseError);

static async Task RunOptionsAsync(Options options)
{
    Console.WriteLine(
        $"Running with arguments: -d {options.Dictionary} -s {options.StartWord} -e {options.EndWord} -o {options.Output}");

    IServiceProvider serviceProvider = new ServiceCollection()
        .AddSingleton<IWordList, WordList>()
        .AddTransient<IFileManager, FileManager>()
        //.AddTransient<IWordValidator, WordValidator>()
        .AddTransient<IPathFinder, PathFinder>()
        .BuildServiceProvider();
    
    ValidateArgs(options, serviceProvider.GetRequiredService<IFileManager>());
    
    var wordDictionary = serviceProvider.GetRequiredService<IWordList>();
    var pathFinder = serviceProvider.GetRequiredService<IPathFinder>();

    var words = await wordDictionary.ReadFromFileAsync(options.Dictionary, options.StartWord.Length);

    var shortestPath = pathFinder.FindShortestPath(words, options.StartWord, options.EndWord);

    //Generate output

    //remove this???
    Environment.Exit(0);
}

static void ValidateArgs(Options options, IFileManager fileManager)
{
    if (!fileManager.FileExists(options.Dictionary))
        throw new FileNotFoundException($"Dictionary file not found at: {options.Dictionary}");

    if (options.StartWord.Length != options.EndWord.Length)
        throw new ArgumentException("Start and end words must be of equal length");
}

static void HandleParseError(IEnumerable<Error> _)
{
    Environment.Exit(1);
}

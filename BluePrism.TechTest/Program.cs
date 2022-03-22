// See https://aka.ms/new-console-template for more information
using BluePrism.TechTest.Library.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using CommandLine;
using BluePrism.TechTest.Console;
using BluePrism.TechTest.Library;
using System.IO.Abstractions;

var parserResult = Parser.Default.ParseArguments<Options>(args);
await parserResult.WithParsedAsync(RunOptionsAsync);
parserResult.WithNotParsed(HandleParseError);

static async Task RunOptionsAsync(Options options)
{
    Console.WriteLine(
        $"Running with arguments: -d {options.Dictionary} -s {options.StartWord} -e {options.EndWord} -o {options.Output}");

    IServiceProvider serviceProvider = new ServiceCollection()
        .AddTransient<IFileSystem, FileSystem>()
        .AddSingleton<IWordList, WordList>()
        .AddTransient<IPathFinder, PathFinder>()
        .BuildServiceProvider();
    
    ValidateArgs(options);
    
    var wordDictionary = serviceProvider.GetRequiredService<IWordList>();
    var pathFinder = serviceProvider.GetRequiredService<IPathFinder>();

    try
    {
        var words = await wordDictionary.ReadFromFileAsync(options.Dictionary, options.StartWord.Length);

        var shortestPath = pathFinder.FindShortestPath(words, options.StartWord, options.EndWord);

        //Generate output


    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Environment.Exit(1);
    }

    Environment.Exit(0);
}

static void ValidateArgs(Options options)
{
    if (options.StartWord.Length != options.EndWord.Length)
        throw new ArgumentException("Start and end words must be of equal length");
}

static void HandleParseError(IEnumerable<Error> _)
{
    Environment.Exit(1);
}

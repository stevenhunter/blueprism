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
    IServiceProvider serviceProvider = new ServiceCollection()
        .AddTransient<IFileSystem, FileSystem>()
        .AddSingleton<IWordRepository, WordRepository>()
        .AddTransient<IOutputWriter, OutputWriter>()
        .AddTransient<IWordService, WordService>()
        .BuildServiceProvider();

    try
    {
        ValidateArgs(options);
    
        var shortestPath = await serviceProvider.GetRequiredService<IWordService>()
            .FindShortestPathAsync(options.Dictionary, options.StartWord, options.EndWord, options.Output);

        PrintOutput(shortestPath, options);        
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

static void PrintOutput(string[] shortestPath, Options options)
{
    if (shortestPath.Length > 0)
    {
        Console.Write($"Success! Shortest path found from {options.StartWord} to {options.EndWord} is: ");

        for (var i = 0; i < shortestPath.Length; i++)
        {
            Console.Write(shortestPath[i]);
            if (i < shortestPath.Length - 1)
                Console.Write(" -> ");
            else
                Console.WriteLine(string.Empty);
        }

        if (string.IsNullOrWhiteSpace(options.Output)) return;
        
        Console.WriteLine(string.Empty);
        Console.WriteLine($"Output written to file {options.Output}");
    }
    else
    {
        Console.WriteLine($"No path found from {options.StartWord} to {options.EndWord}");
    }
}

static void HandleParseError(IEnumerable<Error> _)
{
    Environment.Exit(1);
}

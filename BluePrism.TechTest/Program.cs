using System.IO.Abstractions;
using BluePrism.TechTest.Console;
using BluePrism.TechTest.Library;
using BluePrism.TechTest.Library.Interfaces;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;

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
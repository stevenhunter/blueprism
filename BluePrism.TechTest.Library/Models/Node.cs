namespace BluePrism.TechTest.Library.Models;

public class Node
{
    public Node(string word)
    {
        if (string.IsNullOrWhiteSpace(word)) throw new ArgumentNullException(nameof(word));

        Word = word;
    }

    public string Word { get; init; }

    public Node? Parent { get; init; }
}
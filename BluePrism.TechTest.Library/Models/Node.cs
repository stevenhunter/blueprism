namespace BluePrism.TechTest.Library.Models
{
    public class Node
    {
        public string Word { get;  }

        public Node? Parent { get; }

        public Node(string word, Node? parent)
        {
            if (string.IsNullOrWhiteSpace(word)) 
                throw new ArgumentNullException(nameof(word), "Value cannot be null, empty or whitespace");

            Word = word;
            Parent = parent;
        }
    }
}

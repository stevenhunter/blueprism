namespace BluePrism.TechTest.Library.Interfaces
{
    public interface IOutputWriter
    {
        Task WriteToFileAsync(string path, IEnumerable<string> output);
    }
}

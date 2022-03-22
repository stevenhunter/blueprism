namespace BluePrism.TechTest.Library.Interfaces
{
    public interface IFileManager
    {
        StreamReader StreamReader(string path);

        bool FileExists(string path);
    }
}

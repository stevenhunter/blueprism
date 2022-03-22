using BluePrism.TechTest.Library.Interfaces;

namespace BluePrism.TechTest.Library
{
    public class FileManager : IFileManager
    {
        public StreamReader StreamReader(string path)
        {
            return new StreamReader(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}

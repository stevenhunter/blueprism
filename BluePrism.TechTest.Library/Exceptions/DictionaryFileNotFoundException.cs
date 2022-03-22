using System.Diagnostics.CodeAnalysis;

namespace BluePrism.TechTest.Library.Exceptions
{
    [Serializable, ExcludeFromCodeCoverage]
    public class DictionaryFileNotFoundException : Exception
    {
        public DictionaryFileNotFoundException() { }
        public DictionaryFileNotFoundException(string message) : base(message) { }
        public DictionaryFileNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected DictionaryFileNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

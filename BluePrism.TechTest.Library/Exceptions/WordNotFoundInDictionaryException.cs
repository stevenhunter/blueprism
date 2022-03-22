using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace BluePrism.TechTest.Library.Exceptions
{
    [Serializable, ExcludeFromCodeCoverage]
    public class WordNotFoundInDictionaryException : Exception
    {
        public WordNotFoundInDictionaryException() { }

        public WordNotFoundInDictionaryException(string message) : base(message) { }

        public WordNotFoundInDictionaryException(string message, Exception inner) : base(message, inner) { }

        protected WordNotFoundInDictionaryException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

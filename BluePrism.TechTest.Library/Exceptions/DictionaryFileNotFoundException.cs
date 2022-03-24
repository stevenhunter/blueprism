using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace BluePrism.TechTest.Library.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class DictionaryFileNotFoundException : Exception
{
    public DictionaryFileNotFoundException()
    {
    }

    public DictionaryFileNotFoundException(string message) : base(message)
    {
    }

    public DictionaryFileNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }

    protected DictionaryFileNotFoundException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}
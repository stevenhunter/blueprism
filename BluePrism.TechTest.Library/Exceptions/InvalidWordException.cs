using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace BluePrism.TechTest.Library.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class InvalidWordException : Exception
{
    public InvalidWordException()
    {
    }

    public InvalidWordException(string message) : base(message)
    {
    }

    public InvalidWordException(string message, Exception inner) : base(message, inner)
    {
    }

    protected InvalidWordException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}


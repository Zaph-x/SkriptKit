using System;
using System.Runtime.Serialization;

namespace SkriptKit.Core.Exceptions
{
    public class InvalidShellException : Exception
    {
        public InvalidShellException(string message) : base(message)
        {
        }

        public InvalidShellException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidShellException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
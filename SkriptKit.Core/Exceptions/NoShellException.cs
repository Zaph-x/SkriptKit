using System;
using System.Runtime.Serialization;

namespace SkriptKit.Core.Exceptions
{
    public class NoShellException : Exception
    {
        public NoShellException(string message) : base(message)
        {
        }

        public NoShellException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoShellException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
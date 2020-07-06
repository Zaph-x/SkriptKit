using System;
using System.Runtime.Serialization;

namespace SkriptKit.Core.Exceptions
{
    public class InvalidOSException : Exception
    {
        public InvalidOSException(string message) : base(message)
        {
        }

        public InvalidOSException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidOSException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
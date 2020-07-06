using System;
using System.Runtime.Serialization;

namespace SkriptKit.Core.Exceptions
{
    public class InsufficientPermissionsException : Exception
    {
        public InsufficientPermissionsException(string message) : base(message)
        {
        }

        public InsufficientPermissionsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InsufficientPermissionsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace Business.Validation
{
    [Serializable]
    public class BadCredentialsException : Exception
    {
        public BadCredentialsException()
        {
        }

        protected BadCredentialsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BadCredentialsException(string? message) : base(message)
        {
        }

        public BadCredentialsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
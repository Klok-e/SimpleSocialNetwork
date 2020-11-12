using System;
using System.Runtime.Serialization;

namespace Business.Validation
{
    [Serializable]
    public class SocialException : Exception
    {
        public SocialException()
        {
        }

        protected SocialException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SocialException(string? message) : base(message)
        {
        }

        public SocialException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
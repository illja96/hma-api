using System;
using System.Runtime.Serialization;

namespace HMA.BLL.Tier1.Exceptions.User
{
    [Serializable]
    public class UserEmailNotVerifiedException : Exception
    {
        public UserEmailNotVerifiedException() { }

        public UserEmailNotVerifiedException(string message) : base(message) { }

        public UserEmailNotVerifiedException(string message, Exception innerException) : base(message, innerException) { }

        protected UserEmailNotVerifiedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

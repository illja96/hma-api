using System;
using System.Runtime.Serialization;

namespace HMA.BLL.Tier1.Exceptions.User
{
    [Serializable]
    public class UserDuplicateInsertionException : Exception
    {
        public UserDuplicateInsertionException() { }

        public UserDuplicateInsertionException(string message) : base(message) { }

        public UserDuplicateInsertionException(string message, Exception innerException) : base(message, innerException) { }

        protected UserDuplicateInsertionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

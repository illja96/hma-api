using System;
using System.Runtime.Serialization;

namespace HMA.BLL.Tier1.Exceptions.Invite
{
    [Serializable]
    public class HouseInviteDuplicateInsertionException : Exception
    {
        public HouseInviteDuplicateInsertionException() { }

        public HouseInviteDuplicateInsertionException(string message) : base(message) { }

        public HouseInviteDuplicateInsertionException(string message, Exception innerException) : base(message, innerException) { }

        protected HouseInviteDuplicateInsertionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

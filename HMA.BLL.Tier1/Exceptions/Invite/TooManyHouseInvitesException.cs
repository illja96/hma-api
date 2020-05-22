using System;
using System.Runtime.Serialization;

namespace HMA.BLL.Tier1.Exceptions.Invite
{
    [Serializable]
    public class TooManyHouseInvitesException : Exception
    {
        public TooManyHouseInvitesException() { }

        public TooManyHouseInvitesException(string message) : base(message) { }

        public TooManyHouseInvitesException(string message, Exception innerException) : base(message, innerException) { }

        protected TooManyHouseInvitesException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

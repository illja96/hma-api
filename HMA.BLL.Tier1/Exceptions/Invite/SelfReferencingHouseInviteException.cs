using System;
using System.Runtime.Serialization;

namespace HMA.BLL.Tier1.Exceptions.Invite
{
    [Serializable]
    public class SelfReferencingHouseInviteException : Exception
    {
        public SelfReferencingHouseInviteException() { }

        public SelfReferencingHouseInviteException(string message) : base(message) { }

        public SelfReferencingHouseInviteException(string message, Exception innerException) : base(message, innerException) { }

        protected SelfReferencingHouseInviteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

using System;
using System.Runtime.Serialization;

namespace HMA.BLL.Tier1.Exceptions.Invite
{
    [Serializable]
    public class HouseInviteNotFoundException : Exception
    {
        public HouseInviteNotFoundException() { }

        public HouseInviteNotFoundException(string message) : base(message) { }

        public HouseInviteNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        protected HouseInviteNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

using System;
using System.Runtime.Serialization;

namespace HMA.BLL.Tier1.Exceptions.House
{
    [Serializable]
    public class HouseNotFoundException : Exception
    {
        public HouseNotFoundException() { }

        public HouseNotFoundException(string message) : base(message) { }

        public HouseNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        protected HouseNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

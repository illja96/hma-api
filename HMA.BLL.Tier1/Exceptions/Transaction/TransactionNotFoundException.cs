using System;
using System.Runtime.Serialization;

namespace HMA.BLL.Tier1.Exceptions.Transaction
{
    [Serializable]
    public class TransactionNotFoundException : Exception
    {
        public TransactionNotFoundException() { }

        public TransactionNotFoundException(string message) : base(message) { }

        public TransactionNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        protected TransactionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

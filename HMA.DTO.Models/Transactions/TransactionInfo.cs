using HMA.DTO.Models.Base;
using System;
using System.Collections.Generic;

namespace HMA.DTO.Models.Transactions
{
    public class TransactionInfo : BaseDalModel
    {
        public TransactionType Type { get; set; }

        public decimal CreatedBy { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public bool IsMonthlyRepeatable { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreationDate { get; set; }

        public List<string> Tags { get; set; }
    }
}

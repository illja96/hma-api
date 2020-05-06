using System;
using System.Collections.Generic;

namespace HMA.DTO.ViewModels.Transactions
{
    public class TransactionInfoViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public TransactionTypeViewModel Type { get; set; }

        /// <summary>
        /// Create by
        /// </summary>
        public decimal CreatedBy { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Is monthly repeatable?
        /// </summary>
        public bool IsMonthlyRepeatable { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public List<string> Tags { get; set; }
    }
}

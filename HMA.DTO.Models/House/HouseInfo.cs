using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HMA.DTO.Models.Base;
using HMA.DTO.Models.Transactions;

namespace HMA.DTO.Models.House
{
    [Table("Houses")]
    public class HouseInfo : BaseDalModel
    {
        public decimal OwnerId { get; set; }

        public string Name { get; set; }

        public List<decimal> MemberIds { get; set; }

        public DateTime CreationDate { get; set; }

        public List<TransactionInfo> Transactions { get; set; }

        public HouseInfo()
        {
            MemberIds = new List<decimal>();
            Transactions = new List<TransactionInfo>();
        }
    }
}

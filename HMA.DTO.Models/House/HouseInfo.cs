using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HMA.DTO.Models.Base;

namespace HMA.DTO.Models.House
{
    [Table("Houses")]
    public class HouseInfo : BaseDalModel
    {
        public decimal OwnerId { get; set; }

        public string Name { get; set; }

        public List<decimal> MemberIds { get; set; }

        public DateTime CreationDate { get; set; }
    }
}

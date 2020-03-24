using System.Collections.Generic;
using HMA.DTO.Models.Base;

namespace HMA.DTO.Models.House
{
    public class HouseSimpleInfo : BaseDalModel
    {
        public decimal OwnerId { get; set; }

        public string Name { get; set; }

        public List<decimal> MemberIds { get; set; }
    }
}

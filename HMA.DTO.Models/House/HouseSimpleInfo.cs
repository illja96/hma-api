using System;
using System.Collections.Generic;
using HMA.DTO.Models.Base;
using HMA.DTO.Models.User;

namespace HMA.DTO.Models.House
{
    public class HouseSimpleInfo : BaseDalModel
    {
        public decimal OwnerId { get; set; }

        public UserSimpleInfo OwnerInfo { get; set; }

        public string Name { get; set; }

        public List<decimal> MemberIds { get; set; }

        public List<UserSimpleInfo> MemberInfos { get; set; }

        public DateTime CreationDate { get; set; }
    }
}

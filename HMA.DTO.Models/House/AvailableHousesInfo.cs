using System.Collections.Generic;

namespace HMA.DTO.Models.House
{
    public class AvailableHousesInfo
    {
        public List<HouseSimpleInfo> Owned { get; set; }

        public List<HouseSimpleInfo> MemberOf { get; set; }
    }
}

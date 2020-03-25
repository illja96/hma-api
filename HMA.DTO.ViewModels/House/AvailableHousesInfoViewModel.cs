using System.Collections.Generic;

namespace HMA.DTO.ViewModels.House
{
    public class AvailableHousesInfoViewModel
    {
        public List<HouseSimpleInfoViewModel> Owned { get; set; }

        public List<HouseSimpleInfoViewModel> MemberOf { get; set; }
    }
}

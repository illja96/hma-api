using System.Collections.Generic;

namespace HMA.DTO.ViewModels.House
{
    public class HouseInfosViewModel
    {
        public List<HouseSimpleInfoViewModel> Owned { get; set; }

        public List<HouseSimpleInfoViewModel> MemberOf { get; set; }
    }
}

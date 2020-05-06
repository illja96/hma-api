using System.Collections.Generic;

namespace HMA.DTO.ViewModels.House
{
    public class AvailableHousesInfoViewModel
    {
        /// <summary>
        /// Owned houses
        /// </summary>
        public List<HouseSimpleInfoViewModel> Owned { get; set; }

        /// <summary>
        /// Membership houses
        /// </summary>
        public List<HouseSimpleInfoViewModel> MemberOf { get; set; }
    }
}

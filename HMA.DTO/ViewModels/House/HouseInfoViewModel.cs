using System.Collections.Generic;

namespace HMA.DTO.ViewModels.House
{
    public class HouseInfoViewModel
    {
        public string Id { get; set; }

        public string OwnerId { get; set; }

        public string Name { get; set; }

        public List<string> MemberIds { get; set; }
    }
}

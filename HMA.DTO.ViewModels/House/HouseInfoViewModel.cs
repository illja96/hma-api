using System;
using System.Collections.Generic;
using HMA.DTO.ViewModels.User;

namespace HMA.DTO.ViewModels.House
{
    public class HouseInfoViewModel
    {
        public string Id { get; set; }
        
        public UserSimpleInfoViewModel OwnerInfo { get; set; }

        public string Name { get; set; }

        public List<UserSimpleInfoViewModel> MemberInfos { get; set; }

        public DateTime CreationDate { get; set; }
    }
}

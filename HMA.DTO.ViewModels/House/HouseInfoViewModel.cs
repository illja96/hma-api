using System;
using System.Collections.Generic;
using HMA.DTO.ViewModels.User;

namespace HMA.DTO.ViewModels.House
{
    public class HouseInfoViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Owner info
        /// </summary>
        public UserSimpleInfoViewModel OwnerInfo { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Member infos
        /// </summary>
        public List<UserSimpleInfoViewModel> MemberInfos { get; set; }

        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}

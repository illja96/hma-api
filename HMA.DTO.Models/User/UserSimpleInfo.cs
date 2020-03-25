using System;

namespace HMA.DTO.Models.User
{
    public class UserSimpleInfo
    {
        public decimal GoogleId { get; set; }

        public string Email { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public Uri Picture { get; set; }
    }
}

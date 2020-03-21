using System;
using System.ComponentModel.DataAnnotations.Schema;
using HMA.DTO.Models.Base;

namespace HMA.DTO.Models
{
    [Table("Users")]
    public class UserInfo : BaseDalModel
    {
        public string GoogleId { get; set; }

        public string Email { get; set; }

        public bool EmailVerified { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string Locale { get; set; }

        public Uri Picture { get; set; }
    }
}

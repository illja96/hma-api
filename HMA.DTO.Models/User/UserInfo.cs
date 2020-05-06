using System;
using System.ComponentModel.DataAnnotations.Schema;
using HMA.DTO.Models.Base;

namespace HMA.DTO.Models.User
{
    [Table("Users")]
    public class UserInfo : BaseDalModel
    {
        public decimal GoogleId { get; set; }

        public string Email { get; set; }

        public bool IsEmailVerified { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string Locale { get; set; }

        public Uri PictureUrl { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}

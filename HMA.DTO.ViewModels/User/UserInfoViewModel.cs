using System;

namespace HMA.DTO.ViewModels.User
{
    public class UserInfoViewModel
    {
        public string GoogleId { get; set; }

        public string Email { get; set; }

        public bool EmailVerified { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string Locale { get; set; }

        public string PictureUrl { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}

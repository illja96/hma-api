using System;

namespace HMA.DTO.ViewModels.User
{
    public class UserInfoViewModel
    {
        /// <summary>
        /// Google id
        /// </summary>
        public string GoogleId { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Is email verified?
        /// </summary>
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// Given name
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Family name
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Locale
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Picture URL
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// Registration date
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Last update date
        /// </summary>
        public DateTime LastUpdateDate { get; set; }
    }
}

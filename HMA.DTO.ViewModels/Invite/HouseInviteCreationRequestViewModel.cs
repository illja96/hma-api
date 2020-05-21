using System.ComponentModel.DataAnnotations;

namespace HMA.DTO.ViewModels.Invite
{
    public class HouseInviteCreationRequestViewModel
    {
        /// <summary>
        /// House id
        /// </summary>
        [Required]
        public string HosueId { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace HMA.DTO.ViewModels.House
{
    public class HouseCreationRequestViewModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HMA.DTO.Models.Base;

namespace HMA.DTO.Models
{
    [Table("Houses")]
    public class House : BaseDalModel
    {
        public string Name { get; set; }

        public List<string> UserId { get; set; }
    }
}

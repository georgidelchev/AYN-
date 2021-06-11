using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models
{
    public class Picture : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(PictureExtensionMaxLength)]
        public string Extension { get; set; }

        [Required]
        public string AdId { get; set; }

        public virtual Ad Ad { get; set; }
    }
}

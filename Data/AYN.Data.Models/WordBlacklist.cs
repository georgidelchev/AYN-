using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models
{
    public class WordBlacklist : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(BlacklistWordMaxLength)]
        public string Content { get; set; }
    }
}

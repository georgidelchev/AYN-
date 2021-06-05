using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models
{
    public class SubCategory : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public ICollection<Ad> Ads { get; set; }
            = new HashSet<Ad>();
    }
}

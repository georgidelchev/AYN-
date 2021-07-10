using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models
{
    public class Address : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(AddressNameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Town> Towns { get; set; }
            = new HashSet<Town>();

        public virtual ICollection<Ad> Ads { get; set; }
            = new HashSet<Ad>();
    }
}

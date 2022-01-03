using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models;

public class Town : BaseDeletableModel<int>
{
    [Required]
    [MaxLength(TownNameMaxLength)]
    public string Name { get; set; }

    public ICollection<Ad> Ads { get; set; }
        = new HashSet<Ad>();

    public virtual ICollection<Address> Addresses { get; set; }
        = new HashSet<Address>();
}

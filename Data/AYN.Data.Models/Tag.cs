using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models;

public class Tag : BaseDeletableModel<int>
{
    [Required]
    [MaxLength(TagNameMaxLength)]
    public string Name { get; set; }

    public virtual ICollection<Ad> Ads { get; set; }
        = new HashSet<Ad>();

    public virtual ICollection<Post> Posts { get; set; }
        = new HashSet<Post>();
}

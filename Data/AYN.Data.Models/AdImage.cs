using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

namespace AYN.Data.Models;

public class AdImage : BaseDeletableModel<int>
{
    [Required]
    public string AdId { get; set; }

    public virtual Ad Ad { get; set; }

    [Required]
    public string ImageUrl { get; set; }
}

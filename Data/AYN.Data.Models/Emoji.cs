using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

namespace AYN.Data.Models
{
    public class Emoji : BaseDeletableModel<int>
    {
        [Required]
        public string Image { get; set; }
    }
}

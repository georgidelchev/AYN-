using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models
{
    public class Category : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }
            = new HashSet<Ad>();

        public virtual ICollection<SubCategory> SubCategories { get; set; }
            = new HashSet<SubCategory>();

        public virtual ICollection<Post> Posts { get; set; }
            = new HashSet<Post>();
    }
}

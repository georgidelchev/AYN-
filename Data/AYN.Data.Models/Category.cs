using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Common;
using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models
{
    public class Category : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Ad> Ads { get; set; }
            = new HashSet<Ad>();

        public ICollection<SubCategory> SubCategories { get; set; }
            = new HashSet<SubCategory>();

        public ICollection<Post> Posts { get; set; }
            = new HashSet<Post>();
    }
}

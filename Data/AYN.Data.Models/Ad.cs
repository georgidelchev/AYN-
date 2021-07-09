using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;
using AYN.Data.Models.Enumerations;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models
{
    public class Ad : BaseDeletableModel<string>
    {
        public Ad()
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        [Required]
        [MaxLength(AdNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public decimal? Weight { get; set; }

        [Required]
        public bool IsPromoted { get; set; }

        public DateTime? PromotedOn { get; set; }

        public DateTime? PromotedUntil { get; set; }

        [Required]
        public bool IsArchived { get; set; }

        public DateTime? ArchivedOn { get; set; }

        [Required]
        public string AddedByUserId { get; set; }

        public virtual ApplicationUser AddedByUser { get; set; }

        [Required]
        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        [Required]
        public ProductCondition ProductCondition { get; set; }

        [Required]
        public AdType AdType { get; set; }

        [Required]
        public DeliveryTake DeliveryTake { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }
            = new HashSet<Picture>();

        public virtual ICollection<Tag> Tags { get; set; }
            = new HashSet<Tag>();

        public virtual ICollection<Comment> Comments { get; set; }
            = new HashSet<Comment>();

        public virtual ICollection<Report> Reports { get; set; }
            = new HashSet<Report>();

        public virtual ICollection<UserAdView> UserAdViews { get; set; }
            = new HashSet<UserAdView>();
    }
}

using System;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models
{
    public class Contact : BaseDeletableModel<string>
    {
        public Contact()
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        [Required]
        [MaxLength(ContactTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(ContactContentMaxLength)]
        public string Content { get; set; }

        public string AddedByUserId { get; set; }

        public ApplicationUser AddedByUser { get; set; }
    }
}

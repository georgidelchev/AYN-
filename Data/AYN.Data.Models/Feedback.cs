using System;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models
{
    public class Feedback : BaseDeletableModel<string>
    {
        public Feedback()
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        [Required]
        [MaxLength(FeedbackTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(FeedbackContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string AddedByUserId { get; set; }

        public ApplicationUser AddedByUser { get; set; }
    }
}

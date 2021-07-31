using System.ComponentModel.DataAnnotations;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Feedback
{
    public class CreateFeedbackInputModel
    {
        [Required]
        [MinLength(FeedbackTitleMinLength)]
        [MaxLength(FeedbackTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(FeedbackContentMinLength)]
        [MaxLength(FeedbackContentMaxLength)]
        public string Content { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

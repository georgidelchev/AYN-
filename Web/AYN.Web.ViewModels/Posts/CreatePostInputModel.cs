using System.ComponentModel.DataAnnotations;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Posts
{
    public class CreatePostInputModel
    {
        [Required]
        [MinLength(PostTitleMinLength)]
        [MaxLength(PostTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(PostContentMinLength)]
        [MaxLength(PostContentMaxLength)]
        public string Content { get; set; }
    }
}

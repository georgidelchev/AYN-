using System.ComponentModel.DataAnnotations;

using AYN.Data.Models;
using AYN.Services.Mapping;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Posts
{
    public class EditPostInputModel : IMapFrom<Post>
    {
        public int Id { get; set; }

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

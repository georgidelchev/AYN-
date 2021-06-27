using System.ComponentModel.DataAnnotations;

namespace AYN.Web.ViewModels.PostReacts
{
    public class PostReactInputModel
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        [Range(1, 7)]
        public int ReactValue { get; set; }
    }
}

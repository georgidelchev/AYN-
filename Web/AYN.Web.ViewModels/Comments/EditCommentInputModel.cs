using AYN.Data.Models;

using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Comments;

public class EditCommentInputModel : IMapFrom<Comment>
{
    public string Id { get; set; }

    public string Content { get; set; }
}

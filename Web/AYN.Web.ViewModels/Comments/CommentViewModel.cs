using System;

using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Comments
{
    public class CommentViewModel : IMapFrom<Comment>
    {
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AddedByUserUsername { get; set; }

        public string AddedByUserId { get; set; }
    }
}

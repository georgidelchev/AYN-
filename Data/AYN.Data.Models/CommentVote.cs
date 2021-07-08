using System;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;
using AYN.Data.Models.Enumerations;

namespace AYN.Data.Models
{
    public class CommentVote : BaseDeletableModel<string>
    {
        public CommentVote()
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        [Required]
        public string CommentId { get; set; }

        public Comment Comment { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public CommentVoteValue Value { get; set; }
    }
}

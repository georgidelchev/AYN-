﻿using System;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models
{
    public class Comment : BaseDeletableModel<string>
    {
        public Comment()
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        [Required]
        public string AddedByUserId { get; set; }

        public virtual ApplicationUser AddedByUser { get; set; }

        [Required]
        public int AdId { get; set; }

        public virtual Ad Ad { get; set; }

        [Required]
        [MaxLength(CommentContentMaxLength)]
        public string Content { get; set; }
    }
}
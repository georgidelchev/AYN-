using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

namespace AYN.Data.Models
{
    public class UserRating : BaseModel<string>
    {
        public UserRating()
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public decimal Value { get; set; }
    }
}

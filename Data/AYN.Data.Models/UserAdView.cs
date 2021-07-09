using System;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

namespace AYN.Data.Models
{
    public class UserAdView : BaseDeletableModel<string>
    {
        public UserAdView()
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string AdId { get; set; }

        public virtual Ad Ad { get; set; }
    }
}

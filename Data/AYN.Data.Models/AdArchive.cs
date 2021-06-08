using System;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

namespace AYN.Data.Models
{
    public class AdArchive : BaseDeletableModel<string>
    {
        public AdArchive()
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public int AdId { get; set; }

        public virtual Ad Ad { get; set; }
    }
}

using System;
using System.Collections.Generic;

using AYN.Data.Common.Models;

using Microsoft.AspNetCore.Identity;

namespace AYN.Data.Models
{
    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
            = new HashSet<IdentityUserRole<string>>();

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
            = new HashSet<IdentityUserClaim<string>>();

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
            = new HashSet<IdentityUserLogin<string>>();
    }
}

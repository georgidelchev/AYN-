// ReSharper disable VirtualMemberCallInConstructor
using System;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using Microsoft.AspNetCore.Identity;

namespace AYN.Data.Models
{
    public class ApplicationRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public ApplicationRole()
            : this(null)
        {
        }

        public ApplicationRole(string name)
            : base(name)
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}

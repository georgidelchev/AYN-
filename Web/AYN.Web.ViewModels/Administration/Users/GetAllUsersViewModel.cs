using System;

using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Administration.Users
{
    public class GetAllUsersViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AvatarExtension { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsBanned { get; set; }
    }
}

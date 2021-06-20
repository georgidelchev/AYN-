using System.ComponentModel.DataAnnotations;

using AYN.Data.Models;
using AYN.Services.Mapping;
using Microsoft.AspNetCore.Http;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Users
{
    public class EditUserGeneralInfoViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        [Required]
        [MinLength(ApplicationUserFirstNameMinLength)]
        [MaxLength(ApplicationUserFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(ApplicationUserMiddleNameMinLength)]
        [MaxLength(ApplicationUserMiddleNameMaxLength)]
        public string MiddleName { get; set; }

        [Required]
        [MinLength(ApplicationUserLastNameMinLength)]
        [MaxLength(ApplicationUserLastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MinLength(ApplicationUserAboutMinLength)]
        [MaxLength(ApplicationUserAboutMaxLength)]
        public string About { get; set; }

        public IFormFile Avatar { get; set; }

        public IFormFile Thumbnail { get; set; }
    }
}

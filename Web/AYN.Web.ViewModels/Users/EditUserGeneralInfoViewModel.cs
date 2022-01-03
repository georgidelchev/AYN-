using System;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Models;
using AYN.Data.Models.Enumerations;
using AYN.Services.Mapping;
using Microsoft.AspNetCore.Http;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Users;

public class EditUserGeneralInfoViewModel : IMapFrom<ApplicationUser>
{
    public string Id { get; set; }

    [Required]
    [MinLength(ApplicationUserFirstNameMinLength)]
    [MaxLength(ApplicationUserFirstNameMaxLength)]
    public string FirstName { get; set; }

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

    public string PhoneNumber { get; set; }

    [Url]
    public string FacebookUrl { get; set; }

    [Url]
    public string InstagramUrl { get; set; }

    [Url]
    public string TikTokUrl { get; set; }

    [Url]
    public string TwitterUrl { get; set; }

    [Url]
    public string WebsiteUrl { get; set; }

    [Required]
    public int TownId { get; set; }

    public DateTime? BirthDay { get; set; }

    [Required]
    public Gender Gender { get; set; }

    public IFormFile Avatar { get; set; }

    public IFormFile Thumbnail { get; set; }
}

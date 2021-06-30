using System.ComponentModel.DataAnnotations;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Administration.Users
{
    public class BanUserInputModel
    {
        [Required]
        [MinLength(ApplicationUserBlockReasonMinLength)]
        [MaxLength(ApplicationUserBlockReasonMaxLength)]
        public string BanReason { get; set; }
    }
}

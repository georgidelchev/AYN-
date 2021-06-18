using System.ComponentModel.DataAnnotations;
using AYN.Data.Models.Enumerations;
using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Reports
{
    public class CreateReportInputModel
    {
        [Required]
        [MinLength(ReportContentMinLength)]
        [MaxLength(ReportContentMaxLength)]
        public string Description { get; set; }

        [Required]
        public ReportType ReportType { get; set; }
    }
}

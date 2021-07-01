using System;
using System.ComponentModel.DataAnnotations;

namespace AYN.Web.ViewModels.Administration.Ads
{
    public class PromoteAdInputModel
    {
        public string Id { get; set; }

        [Required]
        public DateTime PromoteUntil { get; set; }
    }
}

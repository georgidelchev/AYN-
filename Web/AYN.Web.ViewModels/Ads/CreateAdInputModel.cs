using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace AYN.Web.ViewModels.Ads
{
    public class CreateAdInputModel : BaseAdInputModel
    {
        [Required]
        public IEnumerable<IFormFile> Pictures { get; set; }
    }
}

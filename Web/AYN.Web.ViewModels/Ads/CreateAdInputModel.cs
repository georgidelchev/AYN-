using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

namespace AYN.Web.ViewModels.Ads
{
    public class CreateAdInputModel : BaseAdInputModel
    {
        public IEnumerable<IFormFile> Pictures { get; set; }
    }
}

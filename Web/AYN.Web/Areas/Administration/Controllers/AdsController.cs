using System.Linq;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Administration.Ads;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers
{
    public class AdsController : AdministrationController
    {
        private readonly IAdsService adsService;

        public AdsController(
            IAdsService adsService)
        {
            this.adsService = adsService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int id = 1)
        {
            var ads = await this.adsService.GetAllAsync<GetAllAdsViewModel>(string.Empty, "createdOnDesc", null);

            var viewModel = new ListAllAdsViewModel()
            {
                AllAds = ads.Skip((id - 1) * 12).Take(12),
                Count = this.adsService.GetCount(),
                ItemsPerPage = 12,
                PageNumber = id,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Archive(string id)
        {

            return this.Redirect("/Administration/Ads/All");
        }

        [HttpPost]
        public async Task<IActionResult> UnArchive(string id)
        {

            return this.Redirect("/Administration/Ads/All");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {

            return this.Redirect("/Administration/Ads/All");
        }

        [HttpPost]
        public async Task<IActionResult> UnDelete(string id)
        {

            return this.Redirect("/Administration/Ads/All");
        }

        [HttpPost]
        public async Task<IActionResult> Promote(string id)
        {

            return this.Redirect("/Administration/Ads/All");
        }

        [HttpPost]
        public async Task<IActionResult> UnPromote(string id)
        {

            return this.Redirect("/Administration/Ads/All");
        }
    }
}

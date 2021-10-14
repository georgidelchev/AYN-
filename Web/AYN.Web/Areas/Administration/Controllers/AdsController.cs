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
        private readonly IWishlistsService wishlistsService;

        public AdsController(
            IAdsService adsService,
            IWishlistsService wishlistsService)
        {
            this.adsService = adsService;
            this.wishlistsService = wishlistsService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int id = 1)
        {
            var ads = await this.adsService.GetAllAsync<GetAllAdsViewModel>(string.Empty, null, "createdOnDesc", null);

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
            await this.adsService.Archive(id);
            return this.Redirect("/Administration/Ads/All");
        }

        [HttpPost]
        public async Task<IActionResult> UnArchive(string id)
        {
            await this.adsService.UnArchive(id);
            return this.Redirect("/Administration/Ads/All");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string returnUrl = "/Administration/Ads/All")
        {
            await this.adsService.Delete(id);
            await this.wishlistsService.DeleteAsync(id);
            return this.Redirect(returnUrl);
        }

        [HttpGet]
        public IActionResult Promote()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Promote(PromoteAdInputModel input, string id)
        {
            await this.adsService.Promote(input.PromoteUntil, id);
            return this.Redirect("/Administration/Ads/All");
        }

        [HttpPost]
        public async Task<IActionResult> UnPromote(string id)
        {
            await this.adsService.UnPromote(id);
            return this.Redirect("/Administration/Ads/All");
        }
    }
}

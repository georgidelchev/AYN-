﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.Validators;
using AYN.Web.ViewModels.Ads;
using AYN.Web.ViewModels.Categories;
using AYN.Web.ViewModels.SubCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace AYN.Web.Controllers
{
    [Authorize]
    public class AdsController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly ITownsService townsService;
        private readonly IAdsService adsService;
        private readonly IWebHostEnvironment environment;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly IValidator<CreateAdInputModel> createAdValidator;
        private readonly IUserAdsViewsService userAdsViewsService;
        private readonly IAddressesService addressesService;

        public AdsController(
            ICategoriesService categoriesService,
            ITownsService townsService,
            IAdsService adsService,
            IWebHostEnvironment environment,
            ISubCategoriesService subCategoriesService,
            IValidator<CreateAdInputModel> createAdValidator,
            IUserAdsViewsService userAdsViewsService,
            IAddressesService addressesService)
        {
            this.categoriesService = categoriesService;
            this.townsService = townsService;
            this.adsService = adsService;
            this.environment = environment;
            this.subCategoriesService = subCategoriesService;
            this.createAdValidator = createAdValidator;
            this.userAdsViewsService = userAdsViewsService;
            this.addressesService = addressesService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateAdInputModel()
            {
                Categories = await this.categoriesService.GetAllAsKeyValuePairsAsync(),
                Towns = await this.townsService.GetAllAsKeyValuePairsAsync(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdInputModel input)
        {
            var result = this.createAdValidator.Validate(input);

            if (result is not null)
            {
                input.Categories = await this.categoriesService.GetAllAsKeyValuePairsAsync();
                input.Towns = await this.townsService.GetAllAsKeyValuePairsAsync();

                this.ModelState.AddModelError(string.Empty, result);
                return this.View(input);
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await this.adsService.CreateAsync(input, userId, this.environment.WebRootPath);
            return this.Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> All(
            string search,
            string town,
            int? categoryId,
            string orderBy = "createdOnDesc",
            int id = 1)
        {
            try
            {
                var ads = await this.adsService
                    .GetAllAsync<GetAdsViewModel>(search, orderBy, categoryId);

                var itemsPerPage = 12;

                var viewModel = new ListAllAdsViewModel()
                {
                    Count = ads.Count(),
                    ItemsPerPage = itemsPerPage,
                    AllFromSearch = ads.Skip((id - 1) * itemsPerPage).Take(itemsPerPage),
                    PageNumber = id,
                    OrderBy = orderBy,
                    Town = town,
                    CategoryId = categoryId,
                    Search = search,
                    TotalResults = ads.Count(),
                    AllCategoriesWithAllSubCategories = new Dictionary<CategoryViewModel, List<SubCategoryViewModel>>(),
                };

                var categories = this.categoriesService
                    .GetAll<CategoryViewModel>();

                foreach (var category in categories)
                {
                    var subCategories = await this.subCategoriesService
                        .GetAllByCategoryId<SubCategoryViewModel>(category.Id)
                        .ToListAsync();

                    var categoryViewModel = new CategoryViewModel()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        PictureExtension = category.PictureExtension,
                    };

                    viewModel.AllCategoriesWithAllSubCategories
                        .Add(categoryViewModel, subCategories);
                }

                return this.View(viewModel);
            }
            catch
            {
                return this.Redirect($"/Ads/All?search={search}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var viewModel = await this.adsService.GetDetails<GetDetailsViewModel>(id);
            await this.userAdsViewsService.CreateAsync(userId, id);

            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!this.adsService.IsUserOwnsGivenAd(userId, id))
            {
                return this.Redirect($"/Ads/Details?id={id}");
            }

            var viewModel = await this.adsService.GetByIdAsync<EditAdInputModel>(id);
            viewModel.Towns = await this.townsService.GetAllAsKeyValuePairsAsync();
            viewModel.Categories = await this.categoriesService.GetAllAsKeyValuePairsAsync();
            viewModel.SubCategories = await this.subCategoriesService.GetAllByCategoryIdAsKeyValuePairsAsync(viewModel.CategoryId);
            viewModel.Addresses = await this.addressesService.GetAllByTownIdAsKeyValuePairsAsync(viewModel.TownId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAdInputModel input, string id)
        {
            await this.adsService.EditAsync(input);
            return this.Redirect($"/Ads/Details?id={id}");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!this.adsService.IsAdExisting(id))
            {
                return this.Redirect("/");
            }

            if (!this.adsService.IsUserOwnsGivenAd(userId, id))
            {
                return this.Redirect($"/");
            }

            await this.adsService.Delete(id);
            return this.Redirect($"/Ads/Details?id={id}");
        }

        [HttpGet]
        public async Task<IActionResult> Promote(string id)
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Promote(string stripeEmail, string stripeToken, string currency)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions()
            {
                Email = stripeEmail,
                Source = stripeToken,
            });

            var charge = charges.Create(new ChargeCreateOptions()
            {
                Amount = 500,
                Description = "Test Payment",
                Currency = "usd",
                Customer = customer.Id,
                ReceiptEmail = stripeEmail,
                Metadata = new Dictionary<string, string>()
                    {{"OrderId", "111"}, {"Postcode", "6000"},},
            });

            switch (charge.Status)
            {
                case "succeeded":
                    {
                        var balanceTransactionId = charge.BalanceTransactionId;
                        return this.View();
                    }
                default:
                    return this.Redirect("/");
            }

            return this.Redirect("/");
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using AYN.Data.Models.Enumerations;
using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Ads;

namespace AYN.Web.Validators
{
    public class CreateAdValidator : IValidator<CreateAdInputModel>
    {
        private readonly ICategoriesService categoriesService;
        private readonly ITownsService townsService;
        private readonly IImageProcessingService imageProcessingService;

        public CreateAdValidator(
            ICategoriesService categoriesService,
            ITownsService townsService,
            IImageProcessingService imageProcessingService)
        {
            this.categoriesService = categoriesService;
            this.townsService = townsService;
            this.imageProcessingService = imageProcessingService;
        }

        public string Validate(CreateAdInputModel entity)
        {
            string errorMessage = null;

            if (!this.categoriesService.IsExisting(entity.CategoryId))
            {
                errorMessage = "Invalid category.";
            }
            else if (!this.categoriesService.IsCategoryContainsGivenSubCategory(entity.CategoryId, entity.SubCategoryId))
            {
                errorMessage = "This category doesn't contains this subCategory.";
            }
            else if (!this.townsService.IsExisting(entity.TownId))
            {
                errorMessage = "Invalid town.";
            }
            else if (!this.townsService.IsTownContainsGivenAddress(entity.TownId, entity.AddressId))
            {
                errorMessage = "This town doesn't contains this address.";
            }
            else if (!Enum.IsDefined(typeof(ProductCondition), entity.ProductCondition))
            {
                errorMessage = "Invalid product condition.";
            }
            else if (!Enum.IsDefined(typeof(DeliveryTake), entity.DeliveryTake))
            {
                errorMessage = "Invalid delivery take.";
            }
            else if (!Enum.IsDefined(typeof(AdType), entity.AdType))
            {
                errorMessage = "Invalid ad type.";
            }
            else
            {
                foreach (var picture in entity.Pictures)
                {
                    var extension = this.imageProcessingService.GetImageExtension(picture);

                    if (!this.imageProcessingService.IsExtensionValid(extension))
                    {
                        errorMessage = "Invalid Image.";
                    }
                }
            }

            return errorMessage;
        }
    }
}

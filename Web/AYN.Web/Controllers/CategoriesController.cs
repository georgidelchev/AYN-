﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Common;
using AYN.Services.Data;
using AYN.Web.ViewModels.Categories;
using AYN.Web.ViewModels.SubCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly IWebHostEnvironment environment;

        public CategoriesController(
            ICategoriesService categoriesService,
            ISubCategoriesService subCategoriesService,
            IWebHostEnvironment environment)
        {
            this.categoriesService = categoriesService;
            this.subCategoriesService = subCategoriesService;
            this.environment = environment;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var wwwrootPath = this.environment
                .WebRootPath;
            try
            {
                await this.categoriesService.CreateAsync(input, wwwrootPath);
            }
            catch (InvalidOperationException ioe)
            {
                this.ModelState.AddModelError(string.Empty, ioe.Message);

                return this.View(input);
            }

            return this.Redirect("/");
        }

        [HttpGet]
        public IActionResult AddSubCategory()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSubCategory(AddSubCategoryViewModel input, int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            try
            {
                await this.categoriesService.AddSubCategoryAsync(input, id);
            }
            catch (InvalidOperationException ioe)
            {
                this.ModelState.AddModelError(string.Empty, ioe.Message);

                return this.View(input);
            }

            return this.Redirect("/");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var viewModel = this.categoriesService
                .GetByIdAsync<EditCategoryInputModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryInputModel input, int id)
        {
            var wwwrootPath = this.environment
                .WebRootPath;

            await this.categoriesService.UpdateAsync(input, id, wwwrootPath);

            return this.Redirect("/");
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult GetSubCategories(int id)
        {
            var sc = this.subCategoriesService
                .GetAllByCategoryId<SubCategoryViewModel>(id);

            return this.Json(sc);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this.categoriesService.DeleteAsync(id);
            }
            catch (InvalidOperationException ioe)
            {
                this.ModelState.AddModelError(string.Empty, ioe.Message);

                return this.Redirect("/");
            }

            return this.Redirect("/");
        }
    }
}

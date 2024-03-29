﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Administration.Categories;
using AYN.Web.ViewModels.Administration.SubCategories;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations;

public class SubCategoriesService : ISubCategoriesService
{
    private readonly IDeletableEntityRepository<SubCategory> subCategoriesRepository;

    public SubCategoriesService(
        IDeletableEntityRepository<SubCategory> subCategoriesRepository)
    {
        this.subCategoriesRepository = subCategoriesRepository;
    }

    public async Task CreateAsync(AddSubCategoryInputModel input, int categoryId)
    {
        if (this.IsSubCategoryExisting(input.Name))
        {
            throw new InvalidOperationException($"The subCategory '{input.Name}' is already existing!");
        }

        var subCategory = new SubCategory
        {
            Name = input.Name,
            CategoryId = categoryId,
        };

        await this.subCategoriesRepository.AddAsync(subCategory);
        await this.subCategoriesRepository.SaveChangesAsync();
    }

    public IQueryable<T> GetAllByCategoryId<T>(int categoryId)
        => this.subCategoriesRepository
            .All()
            .Where(sc => sc.CategoryId == categoryId)
            .OrderBy(sc => sc.Name)
            .To<T>();

    public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync()
        => await this.subCategoriesRepository
            .All()
            .Select(sc => new
            {
                sc.Id,
                sc.Name,
            })
            .OrderBy(sc => sc.Name)
            .Select(sc => new KeyValuePair<string, string>(sc.Id.ToString(), sc.Name))
            .ToListAsync();

    public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllByCategoryIdAsKeyValuePairsAsync(int categoryId)
        => await this.subCategoriesRepository
            .All()
            .Where(sc => sc.CategoryId == categoryId)
            .Select(sc => new
            {
                sc.Id,
                sc.Name,
            })
            .OrderBy(sc => sc.Name)
            .Select(sc => new KeyValuePair<string, string>(sc.Id.ToString(), sc.Name))
            .ToListAsync();

    public async Task<T> GetByIdAsync<T>(int id)
        => await this.subCategoriesRepository
            .All()
            .Where(sc => sc.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();

    public SubCategory Get(int id)
        => this.subCategoriesRepository
            .All()
            .FirstOrDefault(sc => sc.Id == id);

    public async Task DeleteAsync(int id)
    {
        var subCategory = this.subCategoriesRepository
            .All()
            .FirstOrDefault(sc => sc.Id == id);

        this.subCategoriesRepository.Delete(subCategory);
        await this.subCategoriesRepository.SaveChangesAsync();
    }

    public async Task UnDeleteAsync(int id)
    {
        var subCategory = this.subCategoriesRepository
            .AllWithDeleted()
            .FirstOrDefault(sc => sc.Id == id);

        subCategory.IsDeleted = false;
        subCategory.DeletedOn = null;

        this.subCategoriesRepository.Update(subCategory);
        await this.subCategoriesRepository.SaveChangesAsync();
    }

    public async Task EditAsync(EditSubCategoryInputModel input)
    {
        var subCategory = this.subCategoriesRepository
            .All()
            .FirstOrDefault(sc => sc.Id == input.Id);

        subCategory.Name = input.Name;

        this.subCategoriesRepository.Update(subCategory);
        await this.subCategoriesRepository.SaveChangesAsync();
    }

    public bool IsSubCategoryExisting(string subCategoryName)
        => this.subCategoriesRepository
            .All()
            .Any(sc => sc.Name == subCategoryName);

    public bool IsSubCategoryExisting(int id)
        => this.subCategoriesRepository
            .All()
            .Any(sc => sc.Id == id);
}

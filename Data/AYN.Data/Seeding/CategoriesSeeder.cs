using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Models;
using AYN.Data.Seeding.Interfaces;

namespace AYN.Data.Seeding;

public class CategoriesSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (dbContext.Categories.Any())
        {
            return;
        }

        var categories = new List<Category>
        {
            new Category
            {
                Name = "Toys",
                ImageUrl = "https://freepngimg.com/thumb/toy/33903-2-plush-toy-transparent-image.png",
                SubCategories = new List<SubCategory>
                {
                    new SubCategory
                    {
                        Name = "Baby toys",
                    },
                    new SubCategory
                    {
                        Name = "Wooden toys",
                    },
                },
            },
        };

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Models;
using AYN.Data.Seeding.Interfaces;

namespace AYN.Data.Seeding.Seeders;

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
            new()
            {
                Name = "Child and Baby",
                ImageUrl = "https://2rdnmg1qbg403gumla1v9i2h-wpengine.netdna-ssl.com/wp-content/uploads/sites/3/2021/01/BabyFood-1155235287-770x533-1-650x428.jpg",
                SubCategories = new List<SubCategory>
                {
                    new()
                    {
                        Name = "Baby clothes",
                    },
                    new()
                    {
                        Name = "Child clothes",
                    },
                    new()
                    {
                        Name = "Shoes",
                    },
                    new()
                    {
                        Name = "Toys",
                    },
                    new()
                    {
                        Name = "For kid room",
                    },
                    new()
                    {
                        Name = "Baby accessories",
                    },
                    new()
                    {
                        Name = "Child accessories",
                    },
                },
            },
            new()
            {
                Name = "Auto",
                ImageUrl = "https://stimg.cardekho.com/images/carexteriorimages/930x620/Lamborghini/Aventador/6721/Lamborghini-Aventador-SVJ/1621849426405/front-left-side-47.jpg",
                SubCategories = new List<SubCategory>
                {
                    new()
                    {
                        Name = "Cars and SUVs",
                    },
                    new()
                    {
                        Name = "Buses",
                    },
                    new()
                    {
                        Name = "Trucks",
                    },
                    new()
                    {
                        Name = "Specialized equipment",
                    },
                    new()
                    {
                        Name = "Motorcycles and mototechnics",
                    },
                    new()
                    {
                        Name = "Trailers",
                    },
                    new()
                    {
                        Name = "Spare Parts",
                    },
                    new()
                    {
                        Name = "Accessories and consumables",
                    },
                    new()
                    {
                        Name = "Tires and wheels",
                    },
                    new()
                    {
                        Name = "Auto services",
                    },
                    new()
                    {
                        Name = "Water transport",
                    },
                },
            },
            new()
            {
                Name = "Business equipment",
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS4CMIsGvy5eiB0hYVvB7ULSSQ4yBVRyrGc9Q&usqp=CAU",
                SubCategories = new List<SubCategory>
                {
                    new()
                    {
                        Name = "Commercial equipment",
                    },
                    new()
                    {
                        Name = "Hotel and restaurant equipment",
                    },
                    new()
                    {
                        Name = "Hairdressing and beauty equipment",
                    },
                    new()
                    {
                        Name = "Industrial car equipment",
                    },
                    new()
                    {
                        Name = "Agricultural vehicles",
                    },
                    new()
                    {
                        Name = "Medical equipment",
                    },
                    new()
                    {
                        Name = "Tools",
                    },
                    new()
                    {
                        Name = "Machinery and industrial equipment",
                    },
                    new()
                    {
                        Name = "Office furniture and materials",
                    },
                },
            },
            new()
            {
                Name = "Sports, Hobbies",
                ImageUrl = "https://media.npr.org/assets/img/2020/06/10/gettyimages-200199027-001_wide-3ff0f063a2bf1ab01550d3508c816bc43009d215.jpg?s=1400",
                SubCategories = new List<SubCategory>
                {
                    new()
                    {
                        Name = "Books, stationery",
                    },
                    new()
                    {
                        Name = "Sporting goods",
                    },
                    new()
                    {
                        Name = "Movies",
                    },
                    new()
                    {
                        Name = "Music",
                    },
                    new()
                    {
                        Name = "Music instruments",
                    },
                    new()
                    {
                        Name = "Antiques and Collectibles",
                    },
                    new()
                    {
                        Name = "Weapons, hunting and fishing",
                    },
                    new()
                    {
                        Name = "Camping equipment",
                    },
                    new()
                    {
                        Name = "Fun games",
                    },
                    new()
                    {
                        Name = "Electronic cigarettes and hookahs",
                    },
                    new()
                    {
                        Name = "Tickets and Events",
                    },
                    new()
                    {
                        Name = "Food, additives and beverages",
                    },
                },
            },
        };

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
    }
}

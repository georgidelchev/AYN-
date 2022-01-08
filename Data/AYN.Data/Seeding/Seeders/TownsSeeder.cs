using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using AYN.Data.Models;
using AYN.Data.Seeding.Interfaces;

namespace AYN.Data.Seeding.Seeders;

public class TownsSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (!dbContext.Towns.Any())
        {
            var townsUrl = "https://bazar.bg/obiavi/prodazhba-imoti";

            var parser = new HtmlParser();
            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(townsUrl);

            var document = parser.ParseDocument(html);

            var towns = document
                .QuerySelectorAll("#autocompleteLocations a")
                .Select(cu => cu.InnerHtml)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            var townsToAdd = new List<Town>();

            foreach (var currentTown in towns)
            {
                var editedTown = currentTown
                    .Trim('n', 'b', 's', 'p', ';', 'г', 'р', '.', ' ', '&');

                if (editedTown.ToLower().StartsWith("област") ||
                    editedTown.ToLower().StartsWith("извън") ||
                    string.IsNullOrWhiteSpace(editedTown))
                {
                    continue;
                }

                var town = new Town
                {
                    Name = editedTown,
                };

                var neighborhoodUrl = townsUrl + $"/{ConvertCyrillicToLatinLetters(editedTown)}";

                html = await httpClient.GetStringAsync(neighborhoodUrl);
                document = parser.ParseDocument(html);

                var townNeighborhoods = document
                    .QuerySelectorAll(".wrapper .control .item-name")
                    .Select(cn => cn.InnerHtml)
                    .ToList();

                foreach (var neighborhood in townNeighborhoods)
                {
                    town.Addresses.Add(new Address
                    {
                        Name = neighborhood,
                    });
                }

                townsToAdd.Add(town);
            }

            await dbContext.Towns.AddRangeAsync(townsToAdd);
            await dbContext.SaveChangesAsync();
        }
    }

    private static string ConvertCyrillicToLatinLetters(string input)
    {
        if (ConvertEdgeCases(input, out var convertCyrillicToLatinLetters))
        {
            return convertCyrillicToLatinLetters;
        }

        input = input.ToLower();

        var bulgarianLetters = new[]
        {
            "а", "б", "в", "г", "д", "е", "ж", "з", "и", "й", "к", "л", "м", "н", "о",
            "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ь", "ю", "я",
        };

        var latinRepresentationsOfBulgarianLetters = new[]
        {
            "a", "b", "v", "g", "d", "e", "zh", "z", "i", "y", "k", "l", "m", "n", "o",
            "p", "r", "s", "t", "u", "f", "h", "ts", "ch", "sh", "sht", "a", "i", "yu", "a",
        };

        for (var i = 0; i < bulgarianLetters.Length; i++)
        {
            input = input.Replace(bulgarianLetters[i], latinRepresentationsOfBulgarianLetters[i]);
        }

        return input.Replace(' ', '-');
    }

    private static bool ConvertEdgeCases(string input, out string convertCyrillicToLatinLetters)
    {
        switch (input)
        {
            case "Смолян":
            {
                convertCyrillicToLatinLetters = "smolian";
                return true;
            }

            case "Ямбол":
            {
                convertCyrillicToLatinLetters = "iambol";
                return true;
            }
        }

        convertCyrillicToLatinLetters = null;

        return false;
    }
}

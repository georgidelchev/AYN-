using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using AngleSharp.Html.Parser;
using AYN.Data.Models;

namespace AYN.Data.Seeding
{
    public class TownsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Towns.Any())
            {
                var getCitiesUrl = "https://bazar.bg/obiavi/prodazhba-imoti";

                var parser = new HtmlParser();
                var httpClient = new HttpClient();

                var html = await httpClient.GetStringAsync(getCitiesUrl);

                var document = parser.ParseDocument(html);

                var cities = document
                    .QuerySelectorAll("#autocompleteLocations a")
                    .Select(cu => cu.InnerHtml)
                    .ToList();

                var index = 0;

                foreach (var city in cities)
                {
                    if (index++ % 2 == 0)
                    {
                        continue;
                    }

                    var editedCity = city
                        .Trim(new[] { 'n', 'b', 's', 'p', ';', 'г', 'р', '.', ' ', '&' });

                    var town = new Town()
                    {
                        Name = editedCity,
                    };

                    await dbContext.Towns.AddAsync(town);
                    await dbContext.SaveChangesAsync();

                    var cityRepresentationInLatin = ConvertCyrillicToLatinLetters(editedCity);

                    var neighborhoodUrl = getCitiesUrl + $"/{cityRepresentationInLatin}";

                    html = await httpClient.GetStringAsync(neighborhoodUrl);
                    document = parser.ParseDocument(html);

                    var cityNeighborhoods = document
                        .QuerySelectorAll(".wrapper .control .item-name")
                        .Select(cn => cn.InnerHtml)
                        .ToList();

                    foreach (var neighborhood in cityNeighborhoods)
                    {
                        town.Addresses.Add(new Address()
                        {
                            Name = neighborhood,
                        });
                    }

                    await dbContext.SaveChangesAsync();
                }
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
                "а", "б", "в", "г", "д", "е", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п",
                "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ь", "ю", "я",
            };

            var latinRepresentationsOfBulgarianLetters = new[]
            {
                "a", "b", "v", "g", "d", "e", "zh", "z", "i", "y", "k",
                "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "h",
                "ts", "ch", "sh", "sht", "a", "i", "yu", "a",
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
}

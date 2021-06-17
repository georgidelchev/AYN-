﻿using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Ads;

namespace AYN.Services.Data
{
    public interface IAdsService
    {
        Task CreateAsync(CreateAdInputModel input, string userId, string imagePath);

        Task<IEnumerable<T>> GetRecent12AdsAsync<T>();

        Task<IEnumerable<T>> GetAllAsync<T>(string search, string orderBy, int? categoryId);

        int GetCount();

        Task<T> GetDetails<T>(string id);
    }
}

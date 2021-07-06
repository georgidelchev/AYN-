using System;

using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Ads
{
    public class WishlistAdsViewModel : IMapFrom<Ad>
    {
        public string Id { get; set; }

        public string PictureExtension { get; set; }

        public decimal Price { get; set; }

        public bool IsPromoted { get; set; }

        public DateTime? PromotedUntil { get; set; }

        public string AddedByUserId { get; set; }

        public string AddedByUserUsername { get; set; }
    }
}

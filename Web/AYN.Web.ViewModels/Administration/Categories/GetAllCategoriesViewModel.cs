using System;

using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Administration.Categories
{
    public class GetAllCategoriesViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureExtension { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeletedOn { get; set; }
    }
}

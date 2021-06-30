using System;

namespace AYN.Web.Areas.Administration.ViewModels.Panels
{
    public class IndexViewModel
    {
        public Tuple<int, int, int> UsersCounts { get; set; }

        public Tuple<int, int, int, int> AdsCount { get; set; }

        public Tuple<int, int> ReportsCount { get; set; }

        public Tuple<int, int> CategoriesCount { get; set; }
    }
}

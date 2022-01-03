using System;

namespace AYN.Web.ViewModels.Administration.Panels;

public class IndexViewModel
{
    public Tuple<int, int, int> UsersCounts { get; set; }

    public Tuple<int, int, int, int> AdsCount { get; set; }

    public Tuple<int, int> ReportsCount { get; set; }

    public Tuple<int, int> CategoriesCount { get; set; }

    public int EmojisCount { get; set; }

    public int BlacklistedWordsCount { get; set; }
}

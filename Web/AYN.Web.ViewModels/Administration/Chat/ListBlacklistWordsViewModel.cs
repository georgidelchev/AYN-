using System.Collections.Generic;

namespace AYN.Web.ViewModels.Administration.Chat;

public class ListBlacklistWordsViewModel : PagingViewModel
{
    public IEnumerable<BlacklistWordViewModel> BlacklistedWords { get; set; }
}

using System.Collections.Generic;

namespace AYN.Web.ViewModels.Users;

public class ListSuggestionPeopleViewModel
{
    public IEnumerable<GetSuggestionPeopleViewModel> SuggestionPeople { get; set; }
}

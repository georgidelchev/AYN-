using System.Collections.Generic;

namespace AYN.Web.ViewModels.Administration.Users;

public class ListAllUserViewModel : PagingViewModel
{
    public IEnumerable<GetAllUsersViewModel> AllUsers { get; set; }
}

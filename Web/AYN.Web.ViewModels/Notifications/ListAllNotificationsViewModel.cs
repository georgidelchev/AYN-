using System.Collections.Generic;

namespace AYN.Web.ViewModels.Notifications
{
    public class ListAllNotificationsViewModel : PagingViewModel
    {
        public IEnumerable<GetAllNotificationsForUserViewModel> Notifications { get; set; }
    }
}

using System.Collections.Generic;

namespace AYN.Web.ViewModels.Notifications
{
    public class ListAllNotificationsViewModel
    {
        public IEnumerable<GetAllNotificationsForUserViewModel> Notifications { get; set; }
    }
}

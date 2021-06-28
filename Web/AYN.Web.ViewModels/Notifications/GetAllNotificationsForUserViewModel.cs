using System;

using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Notifications
{
    public class GetAllNotificationsForUserViewModel : IMapFrom<Notification>
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedOn { get; set; }

        public string RedirectUrl { get; set; }
    }
}

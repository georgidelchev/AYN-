using System;

using AYN.Data.Models;
using AYN.Data.Models.Enumerations;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Administration.Reports
{
    public class GetAllReportsViewModel : IMapFrom<Report>
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string ReportingUserId { get; set; }

        public string ReportingUserUsername { get; set; }

        public string ReportedUserId { get; set; }

        public string ReportedUserUsername { get; set; }

        public string ReportedAdId { get; set; }

        public string ReportedAdName { get; set; }

        public DateTime CreatedOn { get; set; }

        public ReportType ReportType { get; set; }
    }
}

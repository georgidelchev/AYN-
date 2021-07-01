using System.Collections.Generic;

namespace AYN.Web.ViewModels.Administration.Reports
{
    public class ListAllReportsViewModel : PagingViewModel
    {
        public IEnumerable<GetAllReportsViewModel> AllReports { get; set; }
    }
}

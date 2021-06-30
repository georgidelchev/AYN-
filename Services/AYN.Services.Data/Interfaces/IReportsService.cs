using System.Threading.Tasks;
using AYN.Web.ViewModels.Reports;

namespace AYN.Services.Data.Interfaces
{
    public interface IReportsService
    {
        Task CreateAsync(CreateReportInputModel input, string adId, string reportedUserId, string reportingUserId);
    }
}

using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Web.ViewModels.Reports;

namespace AYN.Services.Data
{
    public class ReportsService : IReportsService
    {
        private readonly IDeletableEntityRepository<Report> reportsRepository;

        public ReportsService(IDeletableEntityRepository<Report> reportsRepository)
        {
            this.reportsRepository = reportsRepository;
        }

        public async Task CreateAsync(CreateReportInputModel input, string adId, string reportedUserId, string reportingUserId)
        {
            var report = new Report()
            {
                Description = input.Description,
                ReportedAdId = adId,
                ReportedUserId = reportedUserId,
                ReportingUserId = reportingUserId,
                ReportType = input.ReportType,
            };

            await this.reportsRepository.AddAsync(report);
            await this.reportsRepository.SaveChangesAsync();
        }
    }
}

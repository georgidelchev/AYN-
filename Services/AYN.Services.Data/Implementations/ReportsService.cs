using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Reports;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations
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

        public async Task<IEnumerable<T>> GetAll<T>()
            => await this.reportsRepository
                .All()
                .OrderByDescending(r => r.CreatedOn)
                .To<T>()
                .ToListAsync();

        public Tuple<int, int> GetCounts()
        {
            var activeReports = this.reportsRepository
                .All()
                .Count(r => !r.IsDeleted);

            var deletedReports = this.reportsRepository
                .AllWithDeleted()
                .Count(r => r.IsDeleted);

            return new Tuple<int, int>(activeReports, deletedReports);
        }
    }
}

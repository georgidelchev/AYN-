using System.Linq;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Administration.Reports;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers
{
    public class ReportsController : AdministrationController
    {
        private readonly IReportsService reportsService;
        private readonly IAdsService adsService;

        public ReportsController(
            IReportsService reportsService,
            IAdsService adsService)
        {
            this.reportsService = reportsService;
            this.adsService = adsService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int id = 1)
        {
            var reports = await this.reportsService.GetAll<GetAllReportsViewModel>();

            var viewModel = new ListAllReportsViewModel()
            {
                Count = this.reportsService.GetCount(),
                PageNumber = id,
                ItemsPerPage = 12,
                AllReports = reports.Skip((id - 1) * 12).Take(12),
            };

            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Approve(string reportId, string adId)
        {
            await this.reportsService.DeleteAllByAdId(adId);
            await this.adsService.Delete(adId);

            return this.Redirect("/Administration/Reports/All");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await this.reportsService.DeleteAllByAdId(id);
            return this.Redirect("/Administration/Reports/All");
        }
    }
}

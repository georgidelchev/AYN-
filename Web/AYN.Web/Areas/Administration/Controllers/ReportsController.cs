using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers
{
    public class ReportsController : AdministrationController
    {
        private readonly IReportsService reportsService;

        public ReportsController(
            IReportsService reportsService)
        {
            this.reportsService = reportsService;
        }

        public async Task<IActionResult> All(int id = 1)
        {
            var viewModel = 1;
            return this.View(viewModel);
        }
    }
}

using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data;
using AYN.Web.ViewModels.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportsService reportsService;

        public ReportsController(IReportsService reportsService)
        {
            this.reportsService = reportsService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateReportInputModel input, string id, string reportedUser)
        {
            var reportingUserId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await this.reportsService.CreateAsync(input, id, reportedUser, reportingUserId);

            return this.Redirect("/");
        }
    }
}

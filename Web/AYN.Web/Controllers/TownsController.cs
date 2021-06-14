using System.Threading.Tasks;

using AYN.Common;
using AYN.Services.Data;
using AYN.Web.ViewModels.Addresses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    public class TownsController : Controller
    {
        private readonly IAddressesService addressesService;

        public TownsController(IAddressesService addressesService)
        {
            this.addressesService = addressesService;
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> GetAddresses(int id)
        {
            var addresses = await this.addressesService
                .GetAllByTownIdAsync<GetAddressesViewModel>(id);

            return this.Json(addresses);
        }
    }
}

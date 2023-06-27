using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Addresses;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers;

public class TownsController : BaseController
{
    private readonly IAddressesService addressesService;

    public TownsController(IAddressesService addressesService)
        => this.addressesService = addressesService;

    [HttpGet]
    public async Task<IActionResult> GetAddresses(int id)
    {
        var addresses = await this.addressesService.GetAllByTownIdAsync<GetAddressesViewModel>(id);
        return this.Json(addresses);
    }
}

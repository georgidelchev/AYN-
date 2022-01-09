using System.Linq;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Administration.Users;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers;

public class UsersController : AdministrationController
{
    private readonly IUsersService usersService;

    public UsersController(
        IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpGet]
    public async Task<IActionResult> All(int id = 1)
    {
        var users = await this.usersService.GetAll<GetAllUsersViewModel>();

        var viewModel = new ListAllUserViewModel
        {
            AllUsers = users.Skip((id - 1) * 12).Take(12),
            Count = this.usersService.GetCounts().Item1,
            ItemsPerPage = 12,
            PageNumber = id,
        };

        return this.View(viewModel);
    }

    [HttpGet]
    public IActionResult Ban()
    {
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> Ban(BanUserInputModel input, string id)
    {
        await this.usersService.Ban(input, id);
        return this.Redirect("/Administration/Users/All");
    }

    [HttpPost]
    public async Task<IActionResult> Unban(string id)
    {
        await this.usersService.Unban(id);
        return this.Redirect("/Administration/Users/All");
    }
}

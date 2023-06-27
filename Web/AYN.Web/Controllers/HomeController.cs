using System.Diagnostics;

using AYN.Web.ViewModels;
using AYN.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return this.View(new IndexViewModel { Search = string.Empty });
    }

    public IActionResult Privacy()
        => this.View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
}

using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    public class UsersController : Controller
    {
        public async Task<IActionResult> Profile()
        {
            return this.View();
        }
    }
}

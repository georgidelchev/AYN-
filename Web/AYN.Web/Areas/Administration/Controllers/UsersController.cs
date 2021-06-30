using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers
{
    public class UsersController : AdministrationController
    {
        public UsersController()
        {
        }

        public async Task<IActionResult> All()
        {
            return this.View();
        }
    }
}

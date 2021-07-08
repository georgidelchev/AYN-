using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    public class MessagesController : Controller
    {
        public MessagesController()
        {

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Chat(int chatConversationId)
        {
            var viewModel = 1;
            return this.View(viewModel);
        }
    }
}

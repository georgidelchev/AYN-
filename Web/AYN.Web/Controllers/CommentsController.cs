using System.Security.Claims;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentsService commentsService;

        public CommentsController(
            ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string content, string adId)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await this.commentsService.Create(content, adId, userId);
            return this.Redirect($"/Ads/Details?id={adId}");
        }
    }
}

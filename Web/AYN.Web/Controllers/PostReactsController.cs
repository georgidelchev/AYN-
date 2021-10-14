using System.Security.Claims;
using System.Threading.Tasks;
using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.PostReacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostReactsController : Controller
    {
        private readonly IPostReactsService postReactsService;

        public PostReactsController(
            IPostReactsService postReactsService)
        {
            this.postReactsService = postReactsService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostReactResponseModel>> Post(PostReactInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await this.postReactsService.SetReactAsync(input.PostId, userId, input.ReactValue);
            var totalReacts = this.postReactsService.GetTotalReacts(input.PostId);

            return new PostReactResponseModel() { TotalReacts = totalReacts };
        }
    }
}

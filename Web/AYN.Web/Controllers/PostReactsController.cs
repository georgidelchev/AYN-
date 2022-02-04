using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.Infrastructure.Extensions;
using AYN.Web.ViewModels.PostReacts;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostReactsController : BaseController
{
    private readonly IPostReactsService postReactsService;

    public PostReactsController(
        IPostReactsService postReactsService)
    {
        this.postReactsService = postReactsService;
    }

    [HttpPost]
    public async Task<ActionResult<PostReactResponseModel>> Post(PostReactInputModel input)
    {
        await this.postReactsService.SetReactAsync(input.PostId, this.User.GetId(), input.ReactValue);
        var totalReacts = this.postReactsService.GetTotalReacts(input.PostId);

        return new PostReactResponseModel { TotalReacts = totalReacts };
    }
}

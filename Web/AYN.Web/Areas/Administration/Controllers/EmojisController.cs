using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Administration.Emojis;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers;

public class EmojisController : AdministrationController
{
    private readonly IEmojisService emojisService;

    public EmojisController(
        IEmojisService emojisService)
    {
        this.emojisService = emojisService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmojiInputModel input)
    {
        if (this.emojisService.IsExisting(input.Emoji))
        {
            this.ModelState.AddModelError(string.Empty, "This emoji is already existing.");
            return this.View();
        }

        await this.emojisService.CreateAsync(input);
        return this.RedirectToAction(nameof(this.All));
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var viewModel = new ListEmojiViewModel
        {
            Emojis = await this.emojisService.GetAll<EmojiViewModel>(),
        };

        return this.View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        await this.emojisService.Delete(id);
        return this.RedirectToAction(nameof(this.All));
    }
}

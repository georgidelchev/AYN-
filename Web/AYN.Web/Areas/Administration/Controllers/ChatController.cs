using System.Linq;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Administration.Chat;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers
{
    public class ChatController : AdministrationController
    {
        private readonly IWordsBlacklistService wordsBlacklistService;

        public ChatController(
            IWordsBlacklistService wordsBlacklistService)
        {
            this.wordsBlacklistService = wordsBlacklistService;
        }

        public async Task<IActionResult> BlacklistedWords(int id = 1)
        {
            var blacklistedWords = await this.wordsBlacklistService.AllAsync<BlacklistWordViewModel>();
            var blacklistWordAsArray = blacklistedWords as BlacklistWordViewModel[] ?? blacklistedWords.ToArray();

            var viewModel = new ListBlacklistWordsViewModel()
            {
                BlacklistedWords = blacklistWordAsArray.Skip((id - 1) * 70).Take(70),
                Count = blacklistWordAsArray.Count(),
                ItemsPerPage = 70,
                PageNumber = id,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> DeleteBlacklistedWord(int id)
        {
            await this.wordsBlacklistService.DeleteAsync(id);
            return this.RedirectToAction(nameof(this.BlacklistedWords));
        }
    }
}

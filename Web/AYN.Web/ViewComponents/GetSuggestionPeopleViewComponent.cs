using System.Threading.Tasks;
using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.ViewComponents
{
    public class GetSuggestionPeopleViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public GetSuggestionPeopleViewComponent(
            IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string currentUserId, string openedUserId)
        {
            var viewModel = new ListSuggestionPeopleViewModel()
            {
                SuggestionPeople = await this.usersService.GetSuggestionPeople<GetSuggestionPeopleViewModel>(currentUserId, openedUserId),
            };

            return this.View(viewModel);
        }
    }
}

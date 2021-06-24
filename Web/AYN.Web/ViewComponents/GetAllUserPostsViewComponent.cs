using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.ViewComponents
{
    public class GetAllUserPostsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return this.View();
        }
    }
}

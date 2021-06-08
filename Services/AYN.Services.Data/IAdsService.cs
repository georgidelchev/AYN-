using System.Threading.Tasks;

using AYN.Web.ViewModels.Ads;

namespace AYN.Services.Data
{
    public interface IAdsService
    {
        Task CreateAsync(CreateAdInputModel input);
    }
}

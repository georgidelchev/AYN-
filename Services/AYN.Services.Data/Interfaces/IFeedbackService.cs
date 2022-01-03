using System.Threading.Tasks;

using AYN.Web.ViewModels.Feedback;

namespace AYN.Services.Data.Interfaces;

public interface IFeedbackService
{
    Task CreateAsync(CreateFeedbackInputModel input, string userId);
}

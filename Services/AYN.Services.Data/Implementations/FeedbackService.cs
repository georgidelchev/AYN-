using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Web.ViewModels.Feedback;

namespace AYN.Services.Data.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IDeletableEntityRepository<Feedback> feedbackRepository;

        public FeedbackService(
            IDeletableEntityRepository<Feedback> feedbackRepository)
        {
            this.feedbackRepository = feedbackRepository;
        }

        public async Task CreateAsync(CreateFeedbackInputModel input, string userId)
        {
            var feedback = new Feedback()
            {
                Email = input.Email,
                AddedByUserId = userId,
                Content = input.Content,
                Title = input.Title,
            };

            await this.feedbackRepository.AddAsync(feedback);
            await this.feedbackRepository.SaveChangesAsync();
        }
    }
}

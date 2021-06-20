using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data
{
    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> applicationUserRepository;
        private readonly IDeletableEntityRepository<FollowerFollowee> followerFolloweesRepository;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> applicationUserRepository,
            IDeletableEntityRepository<FollowerFollowee> followerFolloweesRepository)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.followerFolloweesRepository = followerFolloweesRepository;
        }

        public T GetProfileDetails<T>(string id)
        {
            var user = applicationUserRepository
                .All()
                .Include(au => au.Followers)
                .Include(au => au.Followings)
                .FirstOrDefault(au => au.Id == id);
            ;
            return this.applicationUserRepository
                .All()
                .Include(au => au.Followers)
                .Include(au => au.Followings)
                .Where(au => au.Id == id)
                .Include(au => au.Followers)
                .Include(au => au.Followings)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task Follow(string followerId, string followeeId)
        {
            var followerFollowee = new FollowerFollowee()
            {
                FollowerId = followerId,
                FolloweeId = followeeId,
            };

            await this.followerFolloweesRepository.AddAsync(followerFollowee);
            await this.followerFolloweesRepository.SaveChangesAsync();
        }

        public bool IsAlreadyFollower(string followerId, string followeeId)
            => this.followerFolloweesRepository
                .All()
                .Any(ff => ff.FollowerId == followerId &&
                           ff.FolloweeId == followeeId);
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Administration.Users;
using AYN.Web.ViewModels.Users;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> applicationUserRepository;
        private readonly IDeletableEntityRepository<FollowerFollowee> followerFolloweesRepository;
        private readonly ICloudinaryService cloudinaryService;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> applicationUserRepository,
            IDeletableEntityRepository<FollowerFollowee> followerFolloweesRepository,
            ICloudinaryService cloudinaryService)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.followerFolloweesRepository = followerFolloweesRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
            => await this.applicationUserRepository
                .All()
                .OrderBy(au => au.FirstName)
                .ThenBy(au => au.LastName)
                .To<T>()
                .ToListAsync();

        public async Task<T> GetProfileDetails<T>(string id)
            => await this.applicationUserRepository
                .All()
                .Where(au => au.Id == id)
                .Include(au => au.Followers)
                .Include(au => au.Followings)
                .Include(au => au.Posts)
                .Include(au => au.PostReacts)
                .To<T>()
                .FirstOrDefaultAsync();

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

        public async Task Unfollow(string followerId, string followeeId)
        {
            var followerFollowee = this.followerFolloweesRepository
                .All()
                .FirstOrDefault(ff => ff.FollowerId == followerId &&
                                      ff.FolloweeId == followeeId);

            this.followerFolloweesRepository.HardDelete(followerFollowee);

            await this.followerFolloweesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetFollowers<T>(string userId)
            => await this.followerFolloweesRepository
                .All()
                .Where(ff => ff.FolloweeId == userId)
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<T>> GetFollowings<T>(string userId)
            => await this.followerFolloweesRepository
                .All()
                .Where(ff => ff.FollowerId == userId)
                .To<T>()
                .ToListAsync();

        public bool IsFollower(string followerId, string followeeId)
            => this.followerFolloweesRepository
                .All()
                .Any(ff => ff.FollowerId == followerId &&
                           ff.FolloweeId == followeeId);

        public bool IsUserExisting(string userId)
            => this.applicationUserRepository
                .All()
                .Any(au => au.Id == userId);

        public async Task<string> GenerateDefaultAvatar(string firstName, string lastName)
        {
            var text = $"{firstName[0]}{lastName[0]}".ToUpper();
            return await this.ProcessDefaultImage(text, 192, 192, "Arial", 40, FontStyle.Bold);
        }

        public async Task<string> GenerateDefaultThumbnail(string firstName, string lastName)
        {
            var text = $"AYN - All you need!{Environment.NewLine}{firstName} {lastName}".ToUpper();
            return await this.ProcessDefaultImage(text, 1110, 350, "Arial", 40, FontStyle.Bold);
        }

        public async Task<T> GetByIdAsync<T>(string id)
            => await this.applicationUserRepository
                .All()
                .Where(a => a.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

        public async Task EditAsync(EditUserViewModel model, string wwwRootPath)
        {
            var user = this.applicationUserRepository
                .All()
                .FirstOrDefault(au => au.Id == model.EditUserGeneralInfoViewModel.Id);

            if (model.EditUserGeneralInfoViewModel.Avatar is not null)
            {
                await using var ms = new MemoryStream();
                await model.EditUserGeneralInfoViewModel.Avatar.CopyToAsync(ms);
                var destinationData = ms.ToArray();

                var avatarUrl = await this.cloudinaryService.UploadPictureAsync(destinationData, "avatar", "UsersImages", 192, 192);

                user.AvatarImageUrl = avatarUrl;
            }

            if (model.EditUserGeneralInfoViewModel.Thumbnail is not null)
            {
                await using var ms = new MemoryStream();
                await model.EditUserGeneralInfoViewModel.Thumbnail.CopyToAsync(ms);
                var destinationData = ms.ToArray();

                var thumbnailUrl = await this.cloudinaryService.UploadPictureAsync(destinationData, "thumbnail", "UsersImages", 1110, 350);

                user.ThumbnailImageUrl = thumbnailUrl;
            }

            if (user?.FirstName != model.EditUserGeneralInfoViewModel.FirstName ||
                user?.LastName != model.EditUserGeneralInfoViewModel.LastName)
            {
                await this.GenerateDefaultAvatar(model.EditUserGeneralInfoViewModel.FirstName, model.EditUserGeneralInfoViewModel.LastName);
                await this.GenerateDefaultThumbnail(model.EditUserGeneralInfoViewModel.FirstName, model.EditUserGeneralInfoViewModel.LastName);
            }

            user.FirstName = model.EditUserGeneralInfoViewModel.FirstName;
            user.MiddleName = model.EditUserGeneralInfoViewModel.MiddleName;
            user.LastName = model.EditUserGeneralInfoViewModel.LastName;
            user.About = model.EditUserGeneralInfoViewModel.About;
            user.FacebookUrl = model.EditUserGeneralInfoViewModel.FacebookUrl;
            user.InstagramUrl = model.EditUserGeneralInfoViewModel.InstagramUrl;
            user.TikTokUrl = model.EditUserGeneralInfoViewModel.TikTokUrl;
            user.TwitterUrl = model.EditUserGeneralInfoViewModel.TwitterUrl;
            user.WebsiteUrl = model.EditUserGeneralInfoViewModel.WebsiteUrl;
            user.TownId = model.EditUserGeneralInfoViewModel.TownId;
            user.BirthDay = model.EditUserGeneralInfoViewModel.BirthDay;
            user.Gender = model.EditUserGeneralInfoViewModel.Gender;

            this.applicationUserRepository.Update(user);
            await this.applicationUserRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetSuggestionPeople<T>(string userId, string openedUserId)
            => await this.applicationUserRepository
                .All()
                .Where(au => au.Town.Name == this.applicationUserRepository
                    .All()
                    .FirstOrDefault(au => au.Id == userId).Town.Name &&
                             au.Id != userId &&
                             au.Followings.All(f => f.FollowerId != userId) &&
                             au.Id != openedUserId)
                .To<T>()
                .ToListAsync();

        public Tuple<int, int, int> GetCounts()
        {
            var registeredUsersCount = this.applicationUserRepository
                .AllWithDeleted()
                .Count();

            var bannedUsersCount = this.applicationUserRepository
                .All()
                .Count(au => au.IsBanned);

            var nonBannedUsers = this.applicationUserRepository
                .All()
                .Count(au => !au.IsBanned);

            return new Tuple<int, int, int>(registeredUsersCount, bannedUsersCount, nonBannedUsers);
        }

        public async Task Ban(BanUserInputModel input, string userId)
        {
            var user = this.applicationUserRepository
                .All()
                .FirstOrDefault(au => au.Id == userId);

            user.IsBanned = true;
            user.BannedOn = DateTime.UtcNow;
            user.BlockReason = input.BanReason;

            this.applicationUserRepository.Update(user);
            await this.applicationUserRepository.SaveChangesAsync();
        }

        public async Task Unban(string userId)
        {
            var user = this.applicationUserRepository
                .All()
                .FirstOrDefault(au => au.Id == userId);

            user.IsBanned = false;
            user.BannedOn = null;
            user.BlockReason = null;

            this.applicationUserRepository.Update(user);
            await this.applicationUserRepository.SaveChangesAsync();
        }

        public string GetIdByUsername(string username)
            => this.applicationUserRepository
                .All()
                .FirstOrDefault(a => a.UserName.ToLower() == username.ToLower())
                ?.Id;

        public bool IsEmailTaken(string email)
            => this.applicationUserRepository
                .All()
                .Any(au => au.Email == email);

        private async Task<string> ProcessDefaultImage(string text, int width, int height, string fontName, int emSize, FontStyle fontStyle)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or empty.", nameof(text));
            }

            if (string.IsNullOrEmpty(fontName))
            {
                throw new ArgumentException($"'{nameof(fontName)}' cannot be null or empty.", nameof(fontName));
            }

            var backgroundColors = new List<string> { "3C79B2", "FF8F88", "6FB9FF", "C0CC44", "AFB28C" };

            var backgroundColor = backgroundColors[new Random().Next(0, backgroundColors.Count - 1)];

            var bmp = new Bitmap(width, height);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            var font = new Font(fontName, emSize, fontStyle, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(bmp);

            graphics.Clear((Color)new ColorConverter().ConvertFromString("#" + backgroundColor));

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            graphics.DrawString(text, font, new SolidBrush(Color.WhiteSmoke), new RectangleF(0, 0, width, height), sf);
            graphics.Flush();

            await using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Jpeg);

            var destinationData = ms.ToArray();

            return await this.cloudinaryService.UploadPictureAsync(destinationData, "Avatar", "UsersImages", width, height);
        }
    }
}

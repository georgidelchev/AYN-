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
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Users;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data
{
    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> applicationUserRepository;
        private readonly IDeletableEntityRepository<FollowerFollowee> followerFolloweesRepository;
        private readonly IImageProcessingService imageProcessingService;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> applicationUserRepository,
            IDeletableEntityRepository<FollowerFollowee> followerFolloweesRepository,
            IImageProcessingService imageProcessingService)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.followerFolloweesRepository = followerFolloweesRepository;
            this.imageProcessingService = imageProcessingService;
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

        public async Task Unfollow(string followerId, string followeeId)
        {
            var followerFollowee = this.followerFolloweesRepository
                .All()
                .FirstOrDefault(ff => ff.FollowerId == followerId &&
                                      ff.FolloweeId == followeeId);

            this.followerFolloweesRepository.Delete(followerFollowee);

            await this.followerFolloweesRepository.SaveChangesAsync();
        }

        public bool IsFollower(string followerId, string followeeId)
            => this.followerFolloweesRepository
                .All()
                .Any(ff => ff.FollowerId == followerId &&
                           ff.FolloweeId == followeeId);

        public async Task GenerateDefaultAvatar(string firstName, string lastName, string userId, string wwwRootPath)
        {
            var physicalPath = $"{wwwRootPath}/img/UsersAvatars/";
            Directory.CreateDirectory($"{physicalPath}");
            var fullPhysicalPath = physicalPath + $"{userId}_DEFAULT.png";
            File.Delete(fullPhysicalPath);

            var text = $"{firstName[0]}{lastName[0]}".ToUpper();
            await ProcessDefaultImage(text, fullPhysicalPath, 192, 192, "Arial", 40, FontStyle.Bold);
        }

        public async Task GenerateDefaultThumbnail(string firstName, string lastName, string userId, string wwwRootPath)
        {
            var physicalPath = $"{wwwRootPath}/img/UsersThumbnails/";
            Directory.CreateDirectory($"{physicalPath}");
            var fullPhysicalPath = physicalPath + $"{userId}_DEFAULT.png";
            File.Delete(fullPhysicalPath);

            var text = $"AYN - All you need!{Environment.NewLine}{firstName} {lastName}".ToUpper();
            await ProcessDefaultImage(text, fullPhysicalPath, 1110, 350, "Arial", 40, FontStyle.Bold);
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
                var avatarExtension = this.imageProcessingService.GetImageExtension(model.EditUserGeneralInfoViewModel.Avatar);
                this.imageProcessingService.ValidateImageExtension(avatarExtension);

                var physicalPath = $"{wwwRootPath}/img/UsersAvatars/";
                Directory.CreateDirectory($"{physicalPath}");
                var fullPhysicalPath = physicalPath + $"{user.Id}.{avatarExtension}";
                File.Delete(fullPhysicalPath);

                user.AvatarExtension = avatarExtension;

                await using var fileStream = new FileStream(fullPhysicalPath, FileMode.Create);

                await model.EditUserGeneralInfoViewModel.Avatar.CopyToAsync(fileStream);
                await fileStream.DisposeAsync();

                await this.imageProcessingService.SaveImageLocallyAsync(fullPhysicalPath, 192, 192);
            }

            if (model.EditUserGeneralInfoViewModel.Thumbnail is not null)
            {
            }

            if (user.FirstName != model.EditUserGeneralInfoViewModel.FirstName ||
                user.LastName != model.EditUserGeneralInfoViewModel.LastName)
            {
                await this.GenerateDefaultAvatar(model.EditUserGeneralInfoViewModel.FirstName, model.EditUserGeneralInfoViewModel.LastName, user.Id, wwwRootPath);
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

        private static async Task ProcessDefaultImage(string text, string fullPhysicalPath, int width, int height, string fontName, int emSize, FontStyle fontStyle)
        {
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

            await using var fileStream = new FileStream(fullPhysicalPath, FileMode.Create);

            bmp.Save(fileStream, ImageFormat.Png);
        }
    }
}

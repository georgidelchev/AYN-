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

        public async Task GenerateDefaultAvatar(string firstName, string lastName, string userId, string wwwRootPath)
        {
            var backgroundColors = new List<string> { "3C79B2", "FF8F88", "6FB9FF", "C0CC44", "AFB28C" };
            var avatarString = $"{firstName[0]}{lastName[0]}".ToUpper();
            var randomIndex = new Random().Next(0, backgroundColors.Count - 1);
            var bgColor = backgroundColors[randomIndex];

            var bmp = new Bitmap(192, 192);
            var sf = new StringFormat();

            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            var font = new Font("Arial", 48, FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(bmp);

            graphics.Clear((Color)new ColorConverter().ConvertFromString("#" + bgColor));
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.DrawString(avatarString, font, new SolidBrush(Color.WhiteSmoke), new RectangleF(0, 0, 192, 192), sf);
            graphics.Flush();

            var physicalPath = $"{wwwRootPath}/img/UsersAvatars/";
            Directory.CreateDirectory($"{physicalPath}");

            var fullPhysicalPath = physicalPath + $"{userId}.png";

            await using var fileStream = new FileStream(fullPhysicalPath, FileMode.Create);

            bmp.Save(fileStream, ImageFormat.Png);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;
using AYN.Data.Models.Enumerations;
using Microsoft.AspNetCore.Identity;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models
{
    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        [MaxLength(ApplicationUserSocialContactUrlMaxLength)]
        public string ThumbnailExtension { get; set; }

        [MaxLength(ApplicationUserSocialContactUrlMaxLength)]
        public string AvatarExtension { get; set; }

        [MaxLength(ApplicationUserSocialContactUrlMaxLength)]
        public string FacebookUrl { get; set; }

        [MaxLength(ApplicationUserSocialContactUrlMaxLength)]
        public string InstagramUrl { get; set; }

        [MaxLength(ApplicationUserSocialContactUrlMaxLength)]
        public string TikTokUrl { get; set; }

        [MaxLength(ApplicationUserSocialContactUrlMaxLength)]
        public string TwitterUrl { get; set; }

        [MaxLength(ApplicationUserSocialContactUrlMaxLength)]
        public string WebsiteUrl { get; set; }

        [Required]
        [MaxLength(ApplicationUserAboutMaxLength)]
        public string About { get; set; }

        [Required]
        [MaxLength(ApplicationUserFirstNameMaxLength)]
        public string FirstName { get; set; }

        [MaxLength(ApplicationUserMiddleNameMaxLength)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(ApplicationUserLastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public DateTime? BirthDay { get; set; }

        [Required]
        public Gender Gender { get; set; }

        // Audit info
        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        [Required]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Required]
        public bool IsBanned { get; set; }

        public DateTime? BannedOn { get; set; }

        [MaxLength(ApplicationUserBlockReasonMaxLength)]
        public string BlockReason { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
            = new HashSet<IdentityUserRole<string>>();

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
            = new HashSet<IdentityUserClaim<string>>();

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
            = new HashSet<IdentityUserLogin<string>>();

        public virtual ICollection<Ad> Ads { get; set; }
            = new HashSet<Ad>();

        public virtual ICollection<FollowerFollowee> Followers { get; set; }
            = new HashSet<FollowerFollowee>();

        public virtual ICollection<FollowerFollowee> Followings { get; set; }
            = new HashSet<FollowerFollowee>();

        public virtual ICollection<Post> Posts { get; set; }
            = new HashSet<Post>();

        public virtual ICollection<PostVote> PostVotes { get; set; }
            = new HashSet<PostVote>();

        public virtual ICollection<AdArchive> AdArchives { get; set; }
            = new HashSet<AdArchive>();

        public virtual ICollection<FavoritePost> FavoritePosts { get; set; }
            = new HashSet<FavoritePost>();

        public virtual ICollection<UserRating> UserRatings { get; set; }
            = new HashSet<UserRating>();

        public virtual ICollection<PostReact> PostReacts { get; set; }
            = new HashSet<PostReact>();
    }
}

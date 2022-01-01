using AYN.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AYN.Data.Configurations
{
    public class FollowerFolloweeConfiguration : IEntityTypeConfiguration<FollowerFollowee>
    {
        public void Configure(EntityTypeBuilder<FollowerFollowee> followerFollowee)
        {
            followerFollowee.HasOne(ff => ff.Followee)
                .WithMany(ff => ff.Followings)
                .HasForeignKey(ff => ff.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);

            followerFollowee.HasOne(ff => ff.Follower)
                .WithMany(ff => ff.Followers)
                .HasForeignKey(ff => ff.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

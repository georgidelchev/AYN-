using AYN.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AYN.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> post)
            => post.HasOne(p => p.ApplicationUser)
                .WithMany(a => a.Posts)
                .HasForeignKey(f => f.AddedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}

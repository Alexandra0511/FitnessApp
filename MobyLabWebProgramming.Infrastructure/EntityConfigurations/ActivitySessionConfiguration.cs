using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class ActivitySessionConfiguration : IEntityTypeConfiguration<ActivitySession>
{
    public void Configure(EntityTypeBuilder<ActivitySession> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(x => x.Id);
        builder.Property(e => e.UserId)
            .IsRequired();
        builder.Property(e => e.ActivityTypeId)
            .IsRequired();
        builder.Property(e => e.Duration)
            .IsRequired();
        builder.Property(e => e.CaloriesBurned)
            .IsRequired();

        builder.HasOne(e => e.ActivityType)
            .WithMany(e => e.ActivitySessions)
            .HasForeignKey(e => e.ActivityTypeId);

        ////builder.HasOne(e => e.User)
        ////    .WithMany(e => e.ActivitySessions)
        ////    .HasForeignKey(e => e.UserId);
        //////    .OnDelete(DeleteBehavior.Restrict); 
        builder.HasOne(e => e.User)
            .WithMany(e => e.ActivitySessions)
            .HasForeignKey(e => e.UserId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);

    }
}

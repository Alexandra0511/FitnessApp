using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class UserWorkoutSubscriptionConfiguration : IEntityTypeConfiguration<UserWorkoutSubscription>
{
    public void Configure(EntityTypeBuilder<UserWorkoutSubscription> builder)
    { 
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(x => x.Id);
        builder.Property(e => e.UserId)
            .IsRequired();
        builder.Property(e => e.WorkoutProgramId)
            .IsRequired();

        builder.HasOne(uws => uws.User) 
            .WithMany(u => u.WorkoutSubscriptions) 
            .HasForeignKey(uws => uws.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uws => uws.WorkoutProgram) 
            .WithMany(wp => wp.Subscriptions) 
            .HasForeignKey(uws => uws.WorkoutProgramId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}

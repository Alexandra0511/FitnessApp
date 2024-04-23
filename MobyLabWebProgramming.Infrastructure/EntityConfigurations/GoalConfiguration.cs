using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(x => x.Id);
        builder.Property(e => e.GoalWeight)
            .IsRequired();
        builder.Property(e => e.GoalCaloriesPerDay)
            .IsRequired();
        builder.Property(e => e.GoalSteps)
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithOne(user => user.Goal)
            .HasForeignKey<Goal>(e => e.UserId) // Here the foreign key column is specified.
            .HasPrincipalKey<User>(e => e.Id) // This specifies the referenced key in the referenced table.
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // This specifies the delete behavior when the referenced entity is removed.

    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
	public void Configure(EntityTypeBuilder<UserProfile> builder)
	{
		builder.Property(e => e.Id) // This specifies which property is configured.
			.IsRequired(); // Here it is specified if the property is required, meaning it cannot be null in the database.
		builder.HasKey(x => x.Id); // Here it is specifies that the property Id is the primary key.
		builder.Property(e => e.Age)
			.IsRequired();
		builder.Property(e => e.Weight)
			.IsRequired();
		builder.Property(e => e.Height)
			.IsRequired();

        //builder.HasOne(e => e.User) 
        //	.WithOne(e => e.Profile) 
        //	.HasForeignKey(e => e.UserId) 
        //	.HasPrincipalKey(e => e.Id)
        //	.IsRequired();

        builder.HasOne<User>(up => up.User) 
            .WithOne(u => u.Profile)
            .HasForeignKey<UserProfile>(up => up.UserId)
            .HasPrincipalKey<User>(u => u.Id) 
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

    }
}

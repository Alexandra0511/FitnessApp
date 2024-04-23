using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for a user entity, it will be mapped to a single table and each property will have it's own column except for entity object references also known as navigation properties.
/// </summary>
public class User : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public UserRoleEnum Role { get; set; } = default!;

    public ICollection<UserFile> UserFiles { get; set; } = default!;

    // one-to-one relation
    public UserProfile Profile { get; set; } = default!;
    public Goal Goal { get; set; } = default!;
    // one-to-many relation
    public ICollection<ActivitySession> ActivitySessions { get; set; } = default!;

    // many-to-many relationship
    public ICollection<UserWorkoutSubscription> WorkoutSubscriptions { get; set; } = default!;
}

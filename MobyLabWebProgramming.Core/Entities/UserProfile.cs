namespace MobyLabWebProgramming.Core.Entities;


public class UserProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public int Age { get; set; } = default!;
    public double Weight { get; set; } = default!;
    public double Height { get; set; } = default!;
}
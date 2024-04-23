namespace MobyLabWebProgramming.Core.Entities;

public class ActivitySession : BaseEntity
{
	public Guid UserId { get; set; }
	public User User { get; set; } = default!;
	public Guid ActivityTypeId { get; set; }
	public ActivityType ActivityType { get; set; } = default!;
	public double Duration { get; set; } = default!;
	public int CaloriesBurned { get; set; } = default!;
}

namespace MobyLabWebProgramming.Core.Entities;

public class WorkoutProgram : BaseEntity
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public int DurationDays { get; set; } = default!;
    public int NrCaloriesPerDay { get; set; } = default!;
    public ICollection<UserWorkoutSubscription> Subscriptions { get; set; } = default!;
}
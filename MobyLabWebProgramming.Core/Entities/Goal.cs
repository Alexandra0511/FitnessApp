namespace MobyLabWebProgramming.Core.Entities;

public class Goal : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public int GoalWeight { get; set; } = default!;
    public int GoalCaloriesPerDay { get; set; } = default!;
    public int GoalSteps { get; set; } = default!;
}

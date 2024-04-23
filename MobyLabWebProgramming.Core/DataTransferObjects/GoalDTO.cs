using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class GoalDTO
{
    public Guid Id { get; set; }
    public UserDTO User { get; set; } = default!;
    public int GoalWeight { get; set; } = default!;
    public int GoalCaloriesPerDay { get; set; }
    public int GoalSteps { get; set; }
}

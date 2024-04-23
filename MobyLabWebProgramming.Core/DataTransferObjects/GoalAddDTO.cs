using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class GoalAddDTO
{
    public int GoalWeight { get; set; } = default!;
    public int GoalCaloriesPerDay { get; set; }
    public int GoalSteps { get; set; }
}

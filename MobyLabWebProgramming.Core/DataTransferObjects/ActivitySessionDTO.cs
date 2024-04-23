using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class ActivitySessionDTO 
{
    public Guid Id { get; set; }
    public UserDTO User { get; set; } = default!;
    public ActivityType ActivityType { get; set; } = default!;
    public double Duration { get; set; } = default!;
    public int CaloriesBurned { get; set; } = default!;

}

using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class SubscriptionAddDTO
{
    public Guid UserId { get; set; }
    public Guid WorkoutProgramId { get; set; }
}

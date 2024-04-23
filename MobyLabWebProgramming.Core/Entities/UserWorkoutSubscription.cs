namespace MobyLabWebProgramming.Core.Entities;


public class UserWorkoutSubscription : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public Guid WorkoutProgramId { get; set; }
    public WorkoutProgram WorkoutProgram { get; set; } = default!;
    //public DateTime StartDate { get; set; }
    //public DateTime EndDate { get; set; }
}

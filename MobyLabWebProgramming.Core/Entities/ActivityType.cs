namespace MobyLabWebProgramming.Core.Entities;

public class ActivityType : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public ICollection<ActivitySession> ActivitySessions { get; set; } = default!;

}

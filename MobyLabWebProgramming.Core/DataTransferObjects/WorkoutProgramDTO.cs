namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class WorkoutProgramDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int DurationDays { get; set; } = default!;
    public int NrCaloriesPerDay { get; set; } = default!;
}

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class UserProfileDTO
{

    public Guid Id { get; set; }
    public int Age { get; set; } = default!;
    public double Weight { get; set; } = default!;
    public double Height { get; set; } = default!;
    public UserDTO User { get; set; } = default!;
}

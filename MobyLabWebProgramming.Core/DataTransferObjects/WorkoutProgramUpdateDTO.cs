using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to update a user, the properties besides the id are nullable to indicate that they may not be updated if they are null.
/// </summary>
public record WorkoutProgramUpdateDTO(Guid Id, string? Title = default, string? Description =default, int? DurationDays = default, int? NrCaloriesPerDay = default);
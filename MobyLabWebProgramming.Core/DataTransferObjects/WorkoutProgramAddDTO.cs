using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class WorkoutProgramAddDTO
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int DurationDays { get; set; } = default!;
    public int NrCaloriesPerDay { get; set; } = default!;
}

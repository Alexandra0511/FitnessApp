using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class ActivitySessionAddDTO
{
    public double Duration { get; set; } = default!;
    public int CaloriesBurned { get; set; } = default!;
    public Guid ActivityTypeId { get; set; } = default!;
}

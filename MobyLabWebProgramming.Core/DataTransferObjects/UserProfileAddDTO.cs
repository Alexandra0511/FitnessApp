using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class UserProfileAddDTO
{
    public int Age { get; set; } = default!;
    public double Weight { get; set; } = default!;
    public double Height { get; set; } = default!;
}

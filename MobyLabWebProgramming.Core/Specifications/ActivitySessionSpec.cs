using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class ActivitySessionSpec : BaseSpec<ActivitySessionSpec, ActivitySession>
{
    public ActivitySessionSpec(Guid id) : base(id)
    {
    }

    
}
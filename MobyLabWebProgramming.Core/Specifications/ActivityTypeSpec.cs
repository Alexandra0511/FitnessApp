using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class ActivityTypeSpec : BaseSpec<ActivityTypeSpec, ActivityType>
{
    public ActivityTypeSpec(Guid id) : base(id)
    {
    }

    public ActivityTypeSpec(string name)
    {
        Query.Where(e => e.Name == name);
    }
}
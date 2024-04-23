using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;
using MobyLabWebProgramming.Core.DataTransferObjects;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class GoalSpec : BaseSpec<GoalSpec, Goal>
{
    public GoalSpec(Guid id) : base(id)
    {
    }

    public GoalSpec(Guid id, UserDTO user)
    {
        Query.Where(e => e.UserId == id);
    }
}
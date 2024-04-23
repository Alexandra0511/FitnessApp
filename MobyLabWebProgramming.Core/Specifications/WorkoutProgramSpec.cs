using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class WorkoutProgramSpec : BaseSpec<WorkoutProgramSpec, WorkoutProgram>
{
    public WorkoutProgramSpec(Guid id) : base(id)
    {
    }

    public WorkoutProgramSpec(string title)
    {
        Query.Where(e => e.Title == title);
    }
}
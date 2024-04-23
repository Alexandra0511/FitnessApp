using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// This is a specification to filter the user entities and map it to and UserDTO object via the constructors.
/// Note how the constructors call the base class's constructors. Also, this is a sealed class, meaning it cannot be further derived.
/// </summary>
public sealed class SubscriptionProjectionSpec : BaseSpec<SubscriptionProjectionSpec, UserWorkoutSubscription, SubscriptionDTO>
{
    private string? search;

    /// <summary>
    /// This is the projection/mapping expression to be used by the base class to get UserDTO object from the database.
    /// </summary>
    protected override Expression<Func<UserWorkoutSubscription, SubscriptionDTO>> Spec => e => new()
    {
        Id = e.Id,
        User = new()
        {
            Id = e.User.Id,
            Email = e.User.Email,
            Name = e.User.Name,
            Role = e.User.Role
        },
        WorkoutProgram = new()
        {
            Id = e.WorkoutProgram.Id,
            Title = e.WorkoutProgram.Title,
            DurationDays = e.WorkoutProgram.DurationDays,
            NrCaloriesPerDay = e.WorkoutProgram.NrCaloriesPerDay,
            Description = e.WorkoutProgram.Description ?? ""
        }
    };

    public SubscriptionProjectionSpec(bool orderByCreatedAt = true) : base(orderByCreatedAt)
    {
    }

    public SubscriptionProjectionSpec(Guid id) : base(id)
    {
    }

    public SubscriptionProjectionSpec(Guid userId, UserDTO user) 
    {
        Query.Select(Derived.Spec).Where(e => e.UserId == userId);
    }

    public SubscriptionProjectionSpec(Guid programId, WorkoutProgramDTO program)
    {
        Query.Select(Derived.Spec).Where(e => e.WorkoutProgramId == programId);
    }

    //public SubscriptionProjectionSpec(string? search)
    //{
    //    search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

    //    if (search == null)
    //    {
    //        return;
    //    }

    //    var searchExpr = $"%{search.Replace(" ", "%")}%";

    //    Query.Where(e => EF.Functions.ILike(e.UserId.ToString(), searchExpr)); // This is an example on who database specific expressions can be used via C# expressions.
    //                                                                          // Note that this will be translated to the database something like "where user.Name ilike '%str%'".
    //}

    public SubscriptionProjectionSpec(string? search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            var searchExpr = $"%{search.Replace(" ", "%")}%";

            if (Guid.TryParse(search, out Guid guidSearch))
            {
                Query.Where(e => e.UserId == guidSearch);
            }
            else
            {
                Query.Where(e => EF.Functions.ILike(e.WorkoutProgram.Title, searchExpr));
            }
        }
    }

}

using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class SubscriptionSpec : BaseSpec<SubscriptionSpec, UserWorkoutSubscription>
{
    public SubscriptionSpec(Guid id) : base(id)
    {
    }

    //public UserProfileSpec(string email)
    //{
    //    Query.Where(e => e.Email == email);
    //}
}
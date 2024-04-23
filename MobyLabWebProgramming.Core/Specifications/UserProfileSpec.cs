using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;
using MobyLabWebProgramming.Core.DataTransferObjects;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class UserProfileSpec : BaseSpec<UserProfileSpec, UserProfile>
{
    public UserProfileSpec(Guid id) : base(id)
    {
    }

    public UserProfileSpec(Guid userId, UserDTO user)
    {
        Query.Where(e => e.UserId == userId);
    }
}
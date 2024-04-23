using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Implementation;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class UserWorkoutSubscriptionService : IUserWorkoutSubscriptionService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public UserWorkoutSubscriptionService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<SubscriptionDTO>> GetSubscription(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new SubscriptionProjectionSpec(id), cancellationToken); // Get a user using a specification on the repository.

        return result != null ?
            ServiceResponse<SubscriptionDTO>.ForSuccess(result) :
            ServiceResponse<SubscriptionDTO>.FromError(CommonErrors.SubscriptionNotFound); // Pack the result or error into a ServiceResponse.
    }


    public async Task<ServiceResponse> Subscribe(SubscriptionAddDTO sub, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {

        await _repository.AddAsync(new UserWorkoutSubscription
        {
            WorkoutProgramId = sub.WorkoutProgramId,
            UserId = sub.UserId
        }, cancellationToken); // When the file is saved on the filesystem save the returned file path in the database to identify the file.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Unsubscribe(Guid subId, SubscriptionDTO? requestingSub = default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingSub.User.Id != requestingUser.Id) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own user can delete the user!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<UserWorkoutSubscription>(subId, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<PagedResponse<SubscriptionDTO>>> GetUserWorkoutPrograms(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new SubscriptionProjectionSpec(pagination.Search), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<SubscriptionDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<SubscriptionDTO>>> GetWorkoutProgramUsers(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new SubscriptionProjectionSpec(pagination.Search), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<SubscriptionDTO>>.ForSuccess(result);
    }
}

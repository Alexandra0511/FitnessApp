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

public class ActivitySessionService : IActivitySessionService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public ActivitySessionService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<ActivitySessionDTO>> GetActivitySession(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new ActivitySessionProjectionSpec(id), cancellationToken); // Get a user using a specification on the repository.

        return result != null ?
            ServiceResponse<ActivitySessionDTO>.ForSuccess(result) :
            ServiceResponse<ActivitySessionDTO>.FromError(CommonErrors.ActivitySessionNotFound); // Pack the result or error into a ServiceResponse.
    }

    public async Task<ServiceResponse<PagedResponse<ActivitySessionDTO>>> GetUserActivitySession(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new ActivitySessionProjectionSpec(pagination.Search), cancellationToken);

        return ServiceResponse<PagedResponse<ActivitySessionDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> AddSession(ActivitySessionAddDTO session, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {

        await _repository.AddAsync(new ActivitySession
        {
            Duration = session.Duration,
            CaloriesBurned = session.CaloriesBurned,
            UserId = requestingUser.Id,
            ActivityTypeId = session.ActivityTypeId

        }, cancellationToken); 

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateSession(ActivitySessionUpdateDTO session, ActivitySessionDTO? requestingSession, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingSession.User.Id != requestingUser.Id) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own user can update the activity session!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new ActivitySessionSpec(session.Id), cancellationToken);

        if (entity != null) 
        {
            entity.CaloriesBurned = session.CaloriesBurned ?? entity.CaloriesBurned;
            entity.Duration = session.Duration ?? entity.Duration;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteSession(Guid sessionId, ActivitySessionDTO? requestingSession = default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingSession.User.Id != requestingUser.Id) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own user can delete the activity session!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<ActivitySession>(sessionId, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<PagedResponse<ActivitySessionDTO>>> GetUserActivitySessions(PaginationSearchQueryParams pagination, CancellationToken cancellationToken)
    {
        var result = await _repository.PageAsync(pagination, new ActivitySessionProjectionSpec(pagination.Search), cancellationToken);

        return ServiceResponse<PagedResponse<ActivitySessionDTO>>.ForSuccess(result);
    }
    
}

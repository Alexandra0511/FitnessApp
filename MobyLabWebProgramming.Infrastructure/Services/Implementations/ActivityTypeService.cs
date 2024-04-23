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

public class ActivityTypeService : IActivityTypeService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;
    //private readonly IFileRepository _fileRepository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public ActivityTypeService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<ActivityTypeDTO>> GetActivityType(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new ActivityTypeProjectionSpec(id), cancellationToken); // Get a user using a specification on the repository.

        return result != null ?
            ServiceResponse<ActivityTypeDTO>.ForSuccess(result) :
            ServiceResponse<ActivityTypeDTO>.FromError(CommonErrors.ActivityTypeNotFound); // Pack the result or error into a ServiceResponse.
    }

    public async Task<ServiceResponse<PagedResponse<ActivityTypeDTO>>> GetActivityTypes(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new ActivityTypeProjectionSpec(pagination.Search), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<ActivityTypeDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> CreateActivityType(ActivityTypeAddDTO type, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Personnel) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the personnel can create an activity type!", ErrorCodes.CannotAdd));
        }

        var result = await _repository.GetAsync(new ActivityTypeSpec(type.Name), cancellationToken);

        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The activity type with the specified name already exists", ErrorCodes.ActivityTypeExists));
        }

        await _repository.AddAsync(new ActivityType
        {
            Name = type.Name,
            Description = type.Description
        }, cancellationToken); 

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateActivityType(ActivityTypeUpdateDTO type, ActivityTypeDTO? requestingType, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Personnel) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the personnel can update the activity type!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new ActivityTypeSpec(type.Id), cancellationToken);

        if (entity != null) // Verify if the user is not found, you cannot update an non-existing entity.
        {
            entity.Name = type.Name ?? entity.Name;
            entity.Description = type.Description ?? entity.Description;    

            await _repository.UpdateAsync(entity, cancellationToken); 
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteActivityType(Guid typeId, ActivityTypeDTO? requestingType = default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Personnel) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the personnel can delete the activity type!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<ActivityType>(typeId, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }
}

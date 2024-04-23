using Ardalis.Specification;
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

public class UserProfileService : IUserProfileService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;
    //private readonly IFileRepository _fileRepository;

    /// <summary>
    /// This static method creates the path for a user to where it has to store the files, each user should have an own folder.
    /// </summary>
    //private static string GetFileDirectory(Guid userId) => Path.Join(userId.ToString(), IUserFileService.UserFilesDirectory);

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public UserProfileService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }


    public async Task<ServiceResponse<UserProfileDTO>> GetUserProfile(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new UserProfileProjectionSpec(id), cancellationToken); // Get a user using a specification on the repository.

        return result != null ?
            ServiceResponse<UserProfileDTO>.ForSuccess(result) :
            ServiceResponse<UserProfileDTO>.FromError(CommonErrors.ProfileNotFound); // Pack the result or error into a ServiceResponse.
    }

    public async Task<ServiceResponse<PagedResponse<UserProfileDTO>>> GetUserProfileOnEmail(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new UserProfileProjectionSpec(pagination.Search), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<UserProfileDTO>>.ForSuccess(result);
    }


    public async Task<ServiceResponse> AddProfile(UserProfileAddDTO profile, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new UserProfileSpec(requestingUser.Id, requestingUser), cancellationToken);

        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The user already has a profile!", ErrorCodes.ProfileAlreadyCreated));
        }

        await _repository.AddAsync(new UserProfile
        {
            Age = profile.Age,
            Height = profile.Height,
            Weight = profile.Weight,
            UserId = requestingUser.Id
        }, cancellationToken); // When the file is saved on the filesystem save the returned file path in the database to identify the file.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateProfile(UserProfileUpdateDTO profile, UserProfileDTO? requestingProfile, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingProfile.User.Id != requestingUser.Id) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own user can update the user profile!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new UserProfileSpec(profile.Id), cancellationToken);

        if (entity != null) // Verify if the user is not found, you cannot update an non-existing entity.
        {
            entity.Weight = profile.Weight ?? entity.Weight;
            entity.Height = profile.Height ?? entity.Height;
            entity.Age = profile.Age ?? entity.Age;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteProfile(Guid profileId, UserProfileDTO? requestingProfile=default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingProfile.User.Id != requestingUser.Id) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own user can delete the user profile!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<UserProfile>(profileId, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }
}

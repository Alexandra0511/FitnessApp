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

public class WorkoutProgramService : IWorkoutProgramService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;
    //private readonly IFileRepository _fileRepository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public WorkoutProgramService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<WorkoutProgramDTO>> GetWorkoutProgram(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new WorkoutProgramProjectionSpec(id), cancellationToken); // Get a user using a specification on the repository.

        return result != null ?
            ServiceResponse<WorkoutProgramDTO>.ForSuccess(result) :
            ServiceResponse<WorkoutProgramDTO>.FromError(CommonErrors.WorkoutProgramNotFound); // Pack the result or error into a ServiceResponse.
    }

    public async Task<ServiceResponse<PagedResponse<WorkoutProgramDTO>>> GetWorkoutPrograms(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new WorkoutProgramProjectionSpec(pagination.Search), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<WorkoutProgramDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> CreateWorkoutProgram(WorkoutProgramAddDTO program, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Personnel) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the personnel can create a workout program!", ErrorCodes.CannotAdd));
        }

        await _repository.AddAsync(new WorkoutProgram
        {
            Title = program.Title,
            Description = program.Description,
            NrCaloriesPerDay = program.NrCaloriesPerDay,
            DurationDays = program.DurationDays
        }, cancellationToken); // When the file is saved on the filesystem save the returned file path in the database to identify the file.


        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateWorkoutProgram(WorkoutProgramUpdateDTO program, WorkoutProgramDTO? requestingProgram, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Personnel) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the personnel can update a workout program!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new WorkoutProgramSpec(program.Id), cancellationToken);

        if (entity != null) 
        {
            entity.NrCaloriesPerDay = program.NrCaloriesPerDay ?? entity.NrCaloriesPerDay;
            entity.DurationDays = program.DurationDays ?? entity.DurationDays;
            entity.Title = program.Title ?? entity.Title;
            entity.Description = program.Description ?? entity.Description;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteWorkoutProgram(Guid programId, WorkoutProgramDTO? requestingProgram = default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Personnel) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the personnel can update a workout program!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<Goal>(programId, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }
}

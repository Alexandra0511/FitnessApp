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

public class GoalService : IGoalService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;
    //private readonly IFileRepository _fileRepository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public GoalService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<GoalDTO>> GetUserGoal(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new GoalProjectionSpec(id), cancellationToken); // Get a user using a specification on the repository.

        return result != null ?
            ServiceResponse<GoalDTO>.ForSuccess(result) :
            ServiceResponse<GoalDTO>.FromError(CommonErrors.GoalNotFound); // Pack the result or error into a ServiceResponse.
    }


    public async Task<ServiceResponse> CreateGoal(GoalAddDTO goal, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new GoalSpec(requestingUser.Id, requestingUser), cancellationToken);

        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The user already has a goal set. Try updating it instead!", ErrorCodes.GoalAlreadySet));
        }

        await _repository.AddAsync(new Goal
        {
            GoalCaloriesPerDay = goal.GoalCaloriesPerDay,
            GoalWeight = goal.GoalWeight,
            GoalSteps = goal.GoalSteps,
            UserId = requestingUser.Id
        }, cancellationToken); // When the file is saved on the filesystem save the returned file path in the database to identify the file.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateGoal(GoalUpdateDTO goal, GoalDTO? requestingGoal, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingGoal.User.Id != requestingUser.Id) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own user can update the goal!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new GoalSpec(goal.Id), cancellationToken);

        if (entity != null) // Verify if the user is not found, you cannot update an non-existing entity.
        {
            entity.GoalSteps = goal.GoalSteps ?? entity.GoalSteps;
            entity.GoalCaloriesPerDay = goal.GoalCaloriesPerDay ??entity.GoalCaloriesPerDay;
            entity.GoalWeight = goal.GoalWeight ?? entity.GoalWeight;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteGoal(Guid goalId, GoalDTO? requestingGoal = default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingGoal.User.Id != requestingUser.Id) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own user can delete the goal!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<Goal>(goalId, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }
}

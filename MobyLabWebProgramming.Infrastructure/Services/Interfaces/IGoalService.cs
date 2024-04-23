using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;

public interface IGoalService
{
    //public const string UserFilesDirectory = "UserFiles";

    /// <summary>
    /// GetUserFiles gets the user files as pages from the database.
    /// </summary>
    Task<ServiceResponse<GoalDTO>> GetUserGoal(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse> CreateGoal(GoalAddDTO goal, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateGoal(GoalUpdateDTO goal, GoalDTO? requestingGoal = default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteGoal(Guid goalId, GoalDTO? requestingGoal = default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
}
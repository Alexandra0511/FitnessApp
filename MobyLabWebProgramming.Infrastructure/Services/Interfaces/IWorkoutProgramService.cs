using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

public interface IWorkoutProgramService
{
    //public const string UserFilesDirectory = "UserFiles";

    Task<ServiceResponse<WorkoutProgramDTO>> GetWorkoutProgram(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse> CreateWorkoutProgram(WorkoutProgramAddDTO program, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateWorkoutProgram(WorkoutProgramUpdateDTO program, WorkoutProgramDTO? requestingProgram, UserDTO? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteWorkoutProgram(Guid typeId, WorkoutProgramDTO? requestingProgram = default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<WorkoutProgramDTO>>> GetWorkoutPrograms(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
}
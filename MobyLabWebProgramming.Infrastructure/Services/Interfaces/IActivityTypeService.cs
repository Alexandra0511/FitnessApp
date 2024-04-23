using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

public interface IActivityTypeService
{
    //public const string UserFilesDirectory = "UserFiles";

    Task<ServiceResponse<ActivityTypeDTO>> GetActivityType(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse> CreateActivityType(ActivityTypeAddDTO type, UserDTO? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateActivityType(ActivityTypeUpdateDTO type, ActivityTypeDTO? requestingGoal, UserDTO? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteActivityType(Guid typeId, ActivityTypeDTO? requestingType = default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<ActivityTypeDTO>>> GetActivityTypes(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
}
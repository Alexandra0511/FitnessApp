using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

public interface IActivitySessionService
{
    //public const string UserFilesDirectory = "UserFiles";
    Task<ServiceResponse<PagedResponse<ActivitySessionDTO>>> GetUserActivitySessions(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse<ActivitySessionDTO>> GetActivitySession(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddSession(ActivitySessionAddDTO session, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateSession(ActivitySessionUpdateDTO session, ActivitySessionDTO? requestingSession, UserDTO? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteSession(Guid sessionId, ActivitySessionDTO? requestingProfile = default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<ActivitySessionDTO>>> GetUserActivitySession(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
}
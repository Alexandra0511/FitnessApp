using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

public interface IUserWorkoutSubscriptionService
{
    Task<ServiceResponse<SubscriptionDTO>> GetSubscription(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse> Subscribe(SubscriptionAddDTO sub, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> Unsubscribe(Guid typeId, SubscriptionDTO? requestingSub = default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);

    Task<ServiceResponse<PagedResponse<SubscriptionDTO>>> GetUserWorkoutPrograms(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<SubscriptionDTO>>> GetWorkoutProgramUsers(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
}
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

public interface IUserProfileService
{
    //public const string UserFilesDirectory = "UserFiles";

    Task<ServiceResponse<UserProfileDTO>> GetUserProfile(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddProfile(UserProfileAddDTO profile, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateProfile(UserProfileUpdateDTO profile, UserProfileDTO? requestingProfile, UserDTO? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteProfile(Guid profileId, UserProfileDTO? requestingProfile = default, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<UserProfileDTO>>> GetUserProfileOnEmail(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
}
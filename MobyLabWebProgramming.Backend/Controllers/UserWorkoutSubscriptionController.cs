using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Extensions;
using MobyLabWebProgramming.Infrastructure.Services.Implementations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

/// <summary>
/// This is a controller example for CRUD operations on users.
/// </summary>
[ApiController] // This attribute specifies for the framework to add functionality to the controller such as binding multipart/form-data.
[Route("api/[controller]/[action]")] // The Route attribute prefixes the routes/url paths with template provides as a string, the keywords between [] are used to automatically take the controller and method name.
public class UserWorkoutSubscriptionController : AuthorizedController // Here we use the AuthorizedController as the base class because it derives ControllerBase and also has useful methods to retrieve user information.
{

    private readonly IUserWorkoutSubscriptionService _subscriptionService;
    private readonly IUserService _userService;
    private readonly IWorkoutProgramService _workoutProgramService;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public UserWorkoutSubscriptionController(IUserService userService, IUserWorkoutSubscriptionService subscriptionService, IWorkoutProgramService workoutProgramService) : base(userService) // Also, you may pass constructor parameters to a base class constructor and call as specific constructor from the base class.
    {
        _subscriptionService = subscriptionService;
        _userService = userService;
        _workoutProgramService = workoutProgramService;
    }

    /// <summary>
    /// This method implements the Read operation (R from CRUD) on a user. 
    /// </summary>
    [Authorize] // You need to use this attribute to protect the route access, it will return a Forbidden status code if the JWT is not present or invalid, and also it will decode the JWT token.
    [HttpGet("{id:guid}")] // This attribute will make the controller respond to a HTTP GET request on the route /api/User/GetById/<some_guid>.
    public async Task<ActionResult<RequestResponse<SubscriptionDTO>>> GetById([FromRoute] Guid id) // The FromRoute attribute will bind the id from the route to this parameter.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _subscriptionService.GetSubscription(id)) :
            this.ErrorMessageResult<SubscriptionDTO>(currentUser.Error);
    }

    /// <summary>
    /// This method implements the Read operation (R from CRUD) on a page of users subscribed to a workout program after its name. 
    /// </summary>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<SubscriptionDTO>>>> GetWorkoutsUsers([FromQuery] PaginationSearchQueryParams pagination) // The FromQuery attribute will bind the parameters matching the names of
                                                                                                                                                    // the PaginationSearchQueryParams properties to the object in the method parameter.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _subscriptionService.GetUserWorkoutPrograms(pagination)) :
            this.ErrorMessageResult<PagedResponse<SubscriptionDTO>>(currentUser.Error);
    }

    /// <summary>
    /// This method implements the Read operation (R from CRUD) on a page of workout programs that the user is subscribed to after the user id. 
    /// </summary>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<SubscriptionDTO>>>> GetUsersWorkouts([FromQuery] PaginationSearchQueryParams pagination) // The FromQuery attribute will bind the parameters matching the names of
                                                                                                                                                         // the PaginationSearchQueryParams properties to the object in the method parameter.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _subscriptionService.GetUserWorkoutPrograms(pagination)) :
            this.ErrorMessageResult<PagedResponse<SubscriptionDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpPost] // This attribute will make the controller respond to a HTTP POST request on the route /api/UserFile/Add.
    public async Task<ActionResult<RequestResponse>> Add([FromBody] SubscriptionAddDTO body) // The FromForm attribute will bind each field from the form request to the properties of the UserFileAddDTO parameter.
                                                                                                // For files the property should be IFormFile or IFormFileCollection.
    {
        //var currentUser = await GetCurrentUser();
        var currentUser = await _userService.GetUser(body.UserId);
        return currentUser.Result != null ?
            this.FromServiceResponse(await _subscriptionService.Subscribe(body, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id) // The FromRoute attribute will bind the id from the route to this parameter.
    {
        //var currentUser = await GetCurrentUser();
        var currentProfileUser = await _subscriptionService.GetSubscription(id);

        return currentProfileUser.Result != null ?
            this.FromServiceResponse(await _subscriptionService.Unsubscribe(id)) :
            this.ErrorMessageResult(currentProfileUser.Error);
    }
}
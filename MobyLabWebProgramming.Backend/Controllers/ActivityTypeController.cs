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
/// This is a controller example for CRUD operations on activity types. 
/// Only Admin and Personnel can create/update/delete them.
/// </summary>
[ApiController] 
[Route("api/[controller]/[action]")]
public class ActivityTypeController : AuthorizedController // Here we use the AuthorizedController as the base class because it derives ControllerBase and also has useful methods to retrieve user information.
{

    private readonly IActivityTypeService _activityTypeService;
    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public ActivityTypeController(IUserService userService, IActivityTypeService activityTypeService) : base(userService) // Also, you may pass constructor parameters to a base class constructor and call as specific constructor from the base class.
    {
        _activityTypeService = activityTypeService;
    }

    /// <summary>
    /// This method implements the Read operation (R from CRUD) on an activity type. 
    /// </summary>
    [Authorize] // You need to use this attribute to protect the route access, it will return a Forbidden status code if the JWT is not present or invalid, and also it will decode the JWT token.
    [HttpGet("{id:guid}")] // This attribute will make the controller respond to a HTTP GET request on the route /api/User/GetById/<some_guid>.
    public async Task<ActionResult<RequestResponse<ActivityTypeDTO>>> GetById([FromRoute] Guid id) 
    {
        return this.FromServiceResponse(await _activityTypeService.GetActivityType(id));
    }

    /// <summary>
    /// This method implements the Read operation (R from CRUD) on a page of activity types. 
    /// </summary>
    [Authorize]
    [HttpGet] // This attribute will make the controller respond to a HTTP GET request on the route /api/User/GetPage.
    public async Task<ActionResult<RequestResponse<PagedResponse<ActivityTypeDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination) // The FromQuery attribute will bind the parameters matching the names of
                                                                                                                                                   // the PaginationSearchQueryParams properties to the object in the method parameter.
    {
        return this.FromServiceResponse(await _activityTypeService.GetActivityTypes(pagination));
    }


    /// <summary>
    /// This method implements the Create operation (C from CRUD) on an activity type. 
    /// </summary>
    [Authorize]
    [HttpPost] 
    public async Task<ActionResult<RequestResponse>> Add([FromBody] ActivityTypeAddDTO body) 
    {
        var currentUser = await GetCurrentUser();
        return this.FromServiceResponse(await _activityTypeService.CreateActivityType(body, currentUser.Result));
    }

    /// <summary>
    /// This method implements the Update operation (U from CRUD) on an activity type. 
    /// </summary>
    [Authorize]
    [HttpPut] 
    public async Task<ActionResult<RequestResponse>> UpdateActivityType([FromBody] ActivityTypeUpdateDTO goal) // The FromBody attribute indicates that the parameter is deserialized from the JSON body.
    {
        var currentUser = await GetCurrentUser();
        var currentActivityType = await _activityTypeService.GetActivityType(goal.Id);

        return currentActivityType.Result != null ?
            this.FromServiceResponse(await _activityTypeService.UpdateActivityType(goal, currentActivityType.Result, currentUser.Result)) :
            this.ErrorMessageResult(currentActivityType.Error);
    }

    /// <summary>
    /// This method implements the Delete operation (D from CRUD) on an activity type. 
    /// </summary>
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> DeleteActivityType([FromRoute] Guid id) 
    {
        var currentActivityType = await _activityTypeService.GetActivityType(id);

        return currentActivityType.Result != null ?
            this.FromServiceResponse(await _activityTypeService.DeleteActivityType(id)) :
            this.ErrorMessageResult(currentActivityType.Error);
    }
}
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
/// This is a controller example for CRUD operations on activity sessions.
/// </summary>
[ApiController] 
[Route("api/[controller]/[action]")] 
public class ActivitySessionController : AuthorizedController 
{

    private readonly IActivitySessionService _activitySessionService;
    
    public ActivitySessionController(IUserService userService, IActivitySessionService activitySessionService) : base(userService) // Also, you may pass constructor parameters to a base class constructor and call as specific constructor from the base class.
    {
        _activitySessionService = activitySessionService;
    }

    /// <summary>
    /// This method implements the Read operation (R from CRUD) on an activity session. 
    /// </summary>
    [Authorize] // You need to use this attribute to protect the route access, it will return a Forbidden status code if the JWT is not present or invalid, and also it will decode the JWT token.
    [HttpGet("{id:guid}")] // This attribute will make the controller respond to a HTTP GET request on the route /api/User/GetById/<some_guid>.
    public async Task<ActionResult<RequestResponse<ActivitySessionDTO>>> GetById([FromRoute] Guid id) // The FromRoute attribute will bind the id from the route to this parameter.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _activitySessionService.GetActivitySession(id)) :
            this.ErrorMessageResult<ActivitySessionDTO>(currentUser.Error);
    }

    /// <summary>
    /// This method implements the Read operation (R from CRUD) on a page of activity sessions. 
    /// </summary>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ActivitySessionDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination) // The FromQuery attribute will bind the parameters matching the names of
                                                                                                                                         // the PaginationSearchQueryParams properties to the object in the method parameter.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _activitySessionService.GetUserActivitySessions(pagination)) :
            this.ErrorMessageResult<PagedResponse<ActivitySessionDTO>>(currentUser.Error);
    }

    /// <summary>
    /// This method implements the Create operation (C from CRUD) of a user's activity session. 
    /// </summary>
    [Authorize]
    [HttpPost] // This attribute will make the controller respond to a HTTP POST request on the route /api/UserFile/Add.
    public async Task<ActionResult<RequestResponse>> Add([FromBody] ActivitySessionAddDTO body) // The FromForm attribute will bind each field from the form request to the properties of the UserFileAddDTO parameter.
                                                                                            // For files the property should be IFormFile or IFormFileCollection.
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            this.FromServiceResponse(await _activitySessionService.AddSession(body, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// This method implements the Update operation (U from CRUD) on a user's activity session. 
    /// </summary>
    [Authorize]
    [HttpPut] // This attribute will make the controller respond to a HTTP PUT request on the route /api/User/Update.
    public async Task<ActionResult<RequestResponse>> UpdateSession([FromBody] ActivitySessionUpdateDTO userSession) // The FromBody attribute indicates that the parameter is deserialized from the JSON body.
    {
        var currentUser = await GetCurrentUser();
        var currentSessionUser = await _activitySessionService.GetActivitySession(userSession.Id);

        return currentSessionUser.Result != null ?
            this.FromServiceResponse(await _activitySessionService.UpdateSession(userSession, currentSessionUser.Result, currentUser.Result)) :
            this.ErrorMessageResult(currentSessionUser.Error);
    }

    /// <summary>
    /// This method implements the Delete operation (D from CRUD) on a user.
    /// </summary>
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> DeleteSession([FromRoute] Guid id) // The FromRoute attribute will bind the id from the route to this parameter.
    {
        var currentSessionUser = await _activitySessionService.GetActivitySession(id);

        return currentSessionUser.Result != null ?
            this.FromServiceResponse(await _activitySessionService.DeleteSession(id)) :
            this.ErrorMessageResult(currentSessionUser.Error);
    }
}
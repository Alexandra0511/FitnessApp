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
/// This is a controller example for CRUD operations on an user's goal.
/// </summary>
[ApiController] 
[Route("api/[controller]/[action]")] 
public class GoalController : AuthorizedController 
{

    private readonly IGoalService _goalService;
    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public GoalController(IUserService userService, IGoalService goalService) : base(userService) 
    {
        _goalService = goalService;
    }

    /// <summary>
    /// This method implements the Read operation (R from CRUD) on a user's goal. 
    /// </summary>
    [Authorize] // You need to use this attribute to protect the route access, it will return a Forbidden status code if the JWT is not present or invalid, and also it will decode the JWT token.
    [HttpGet("{id:guid}")] // This attribute will make the controller respond to a HTTP GET request on the route /api/User/GetById/<some_guid>.
    public async Task<ActionResult<RequestResponse<GoalDTO>>> GetById([FromRoute] Guid id) // The FromRoute attribute will bind the id from the route to this parameter.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _goalService.GetUserGoal(id)) :
            this.ErrorMessageResult<GoalDTO>(currentUser.Error);
    }

    /// <summary>
    /// This method implements the Create operation (C from CRUD) on a user's goal. 
    /// </summary>
    [Authorize]
    [HttpPost] 
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GoalAddDTO body) 
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            this.FromServiceResponse(await _goalService.CreateGoal(body, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// This method implements the Update operation (U from CRUD) on a user's goal. 
    /// </summary>
    [Authorize]
    [HttpPut] 
    public async Task<ActionResult<RequestResponse>> UpdateGoal([FromBody] GoalUpdateDTO goal) 
    {
        var currentUser = await GetCurrentUser();
        var currentUserGoal = await _goalService.GetUserGoal(goal.Id);

        return currentUserGoal.Result != null ?
            this.FromServiceResponse(await _goalService.UpdateGoal(goal, currentUserGoal.Result, currentUser.Result)) :
            this.ErrorMessageResult(currentUserGoal.Error);
    }

    /// <summary>
    /// This method implements the Delete operation (D from CRUD) on a user's goal. 
    /// </summary>
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> DeleteGoal([FromRoute] Guid id) 
    {
        //var currentUser = await GetCurrentUser();
        var currentUserGoal = await _goalService.GetUserGoal(id);

        return currentUserGoal.Result != null ?
            this.FromServiceResponse(await _goalService.DeleteGoal(id)) :
            this.ErrorMessageResult(currentUserGoal.Error);
    }
}
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
public class WorkoutProgramController : AuthorizedController // Here we use the AuthorizedController as the base class because it derives ControllerBase and also has useful methods to retrieve user information.
{

    private readonly IWorkoutProgramService _workoutProgramService;
    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public WorkoutProgramController(IUserService userService, IWorkoutProgramService workoutProgramService) : base(userService) // Also, you may pass constructor parameters to a base class constructor and call as specific constructor from the base class.
    {
        _workoutProgramService = workoutProgramService;
    }

    /// <summary>
    /// This method implements the Read operation (R from CRUD) on a workout program. 
    /// </summary>
    [Authorize] // You need to use this attribute to protect the route access, it will return a Forbidden status code if the JWT is not present or invalid, and also it will decode the JWT token.
    [HttpGet("{id:guid}")] // This attribute will make the controller respond to a HTTP GET request on the route /api/User/GetById/<some_guid>.
    public async Task<ActionResult<RequestResponse<WorkoutProgramDTO>>> GetById([FromRoute] Guid id) // The FromRoute attribute will bind the id from the route to this parameter.
    {
        return this.FromServiceResponse(await _workoutProgramService.GetWorkoutProgram(id));
    }

    /// <summary>
    /// This method implements the Read operation (R from CRUD) on a page with workout programs. 
    /// </summary>
    [Authorize]
    [HttpGet] // This attribute will make the controller respond to a HTTP GET request on the route /api/User/GetPage.
    public async Task<ActionResult<RequestResponse<PagedResponse<WorkoutProgramDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination) // The FromQuery attribute will bind the parameters matching the names of
                                                                                                                                                   // the PaginationSearchQueryParams properties to the object in the method parameter.
    {
        return this.FromServiceResponse(await _workoutProgramService.GetWorkoutPrograms(pagination));
    }

    /// <summary>
    /// This method implements the Create operation (C from CRUD) on a workout program. 
    /// </summary>
    [Authorize]
    [HttpPost] // This attribute will make the controller respond to a HTTP POST request on the route /api/UserFile/Add.
    public async Task<ActionResult<RequestResponse>> Add([FromBody] WorkoutProgramAddDTO body) // The FromForm attribute will bind each field from the form request to the properties of the UserFileAddDTO parameter
    {
        var currentUser = await GetCurrentUser();
        return this.FromServiceResponse(await _workoutProgramService.CreateWorkoutProgram(body, currentUser.Result));
    }

    /// <summary>
    /// This method implements the Update operation (U from CRUD) on a workout program. 
    /// </summary>
    [Authorize]
    [HttpPut] // This attribute will make the controller respond to a HTTP PUT request on the route /api/User/Update.
    public async Task<ActionResult<RequestResponse>> Update([FromBody] WorkoutProgramUpdateDTO workoutProgram) // The FromBody attribute indicates that the parameter is deserialized from the JSON body.
    {
        var currentUser = await GetCurrentUser();
        var currentWorkoutProgram = await _workoutProgramService.GetWorkoutProgram(workoutProgram.Id);

        return this.FromServiceResponse(await _workoutProgramService.UpdateWorkoutProgram(workoutProgram, currentWorkoutProgram.Result, currentUser.Result));
        
    }

    /// <summary>
    /// This method implements the Delete operation (D from CRUD) on a workout program. 
    /// </summary>
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id) // The FromRoute attribute will bind the id from the route to this parameter.
    {
        var currentProfileUser = await _workoutProgramService.GetWorkoutProgram(id);

        return this.FromServiceResponse(await _workoutProgramService.DeleteWorkoutProgram(id));
    }
}
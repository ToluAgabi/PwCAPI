using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PwCAssessment.DTOs;
using PwCAssessment.DTOs.Requests;
using PwCAssessment.DTOs.Response;
using PwCAssessment.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PwCAssessment.Controllers;

[Authorize(Roles = "Admin")]
[Produces("application/json")]
[Route("api/[controller]/[action]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly ITravelService _travelService;

    public AdminController(ITravelService travelService)
    {
        _travelService = travelService;
    }



    
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<TravelResponseDto>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]

    [SwaggerOperation(Summary = "Get all Travel Requests")]
    [HttpGet]

    public async Task<ActionResult<GenericResponse<string>>> AllTravelRequests()
    {
        var response = await _travelService.GetAllTravelRequestsAsync();
        return StatusCode((int)response.StatusCode, response);

    }

}
    


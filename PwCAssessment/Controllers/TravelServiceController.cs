using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PwCAssessment.DTOs;
using PwCAssessment.DTOs.Requests;
using PwCAssessment.DTOs.Response;
using PwCAssessment.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PwCAssessment.Controllers;

[Authorize]
[Produces("application/json")]
[Route("api/TravelRequests/[action]")]
[ApiController]
public class TravelServiceController : ControllerBase
{
    private readonly ITravelService _travelService;

    public TravelServiceController(ITravelService travelService)
    {
        _travelService = travelService;
    }



    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<string>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]

    [SwaggerOperation(Summary = "Submit a Travel Request")]
    public async Task<ActionResult<GenericResponse<string>>> TravelRequest([FromBody] TravelReqDto model)
    {
        var response = await _travelService.SubmitTravelRequestAsync(model);
        return StatusCode((int)response.StatusCode, response);

    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<string>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]

    [SwaggerOperation(Summary = "Update a Travel Request")]
    [HttpPut, Route("{requestNumber}")]

    public async Task<ActionResult<GenericResponse<string>>> UpdateRequest([FromBody] TravelReqDto model,
        [FromRoute] string requestNumber)
    {
        var response = await _travelService.UpdateTravelRequestAsync(requestNumber, model);
        return StatusCode((int)response.StatusCode, response);

    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<string>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]

    [SwaggerOperation(Summary = "Delete a Travel Request")]
    [HttpDelete, Route("{requestNumber}")]

    public async Task<ActionResult<GenericResponse<string>>> DeleteRequest([FromRoute] string requestNumber)
    {
        var response = await _travelService.DeleteTravelRequestAsync(requestNumber);
        return StatusCode((int)response.StatusCode, response);

    } 
    
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<TravelResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]

    [SwaggerOperation(Summary = "Get a Travel Request by Request Number")]
    [HttpGet, Route("{requestNumber}")]

    public async Task<ActionResult<GenericResponse<string>>> GetRequestByNumber([FromRoute] string requestNumber)
    {
        var response = await _travelService.GetTravelRequestByReqNumberAsync(requestNumber);
        return StatusCode((int)response.StatusCode, response);

    }
    
 
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<SearchResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]

    [SwaggerOperation(Summary = "Search Location By Name")]
    [HttpGet, Route("{searchParam}")]

    public async Task<ActionResult<GenericResponse<string>>> SearchLocationByName([FromRoute] string searchParam)
    {
        var response = await _travelService.SearchLocationByName(searchParam);
        return StatusCode((int)response.StatusCode, response);

    }

}
    


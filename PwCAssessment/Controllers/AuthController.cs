using System.Net;
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
[Route("api/[controller]/[action]")]
[ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _accountService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;

        }
    
       
        
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<LoginResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        
        [SwaggerOperation(Summary = "Authenticate a user")]
        public async Task<ActionResult<GenericResponse<LoginResponse>>> Login([FromBody] LoginViewModel user)
        {
            var loggedUser = await _accountService.LoginAsync(user);
            return StatusCode((int)loggedUser.StatusCode, loggedUser);

        }
    
    [SwaggerOperation(Summary = "Get a new Access Token")]

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequestModel request)
	    {
	    try
		    {
			    var principal = _tokenService.GetPrincipalFromExpiredToken(request.AuthenticationToken);
			    var username = principal.Identity?.Name; //this is mapped to the Name claim by default

			    var user = await _tokenService.GetUserRefreshTokenByEmailAsync(username);

			    if (user == null || user.RefreshToken != request.RefreshToken)
				    return BadRequest(new GenericResponse<object> { Data = null, Message = "Invalid Token Combination", Success = false });

			    var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
			    var newRefreshToken = _tokenService.GenerateRefreshToken();

			    var res = await _tokenService.UpdateUserRefreshTokenAsync(user.Id!, newRefreshToken);

			    if (res == null)
			    {
				    return StatusCode((int)HttpStatusCode.InternalServerError, new GenericResponse<object> { Data = null, Message = "Unable to update refresh token", Success = false });
			    }

			    return Ok(new GenericResponse<object>
			    {
			    Data = new
				    {
				    AccessToken = newJwtToken,
				    RefreshToken = newRefreshToken
				    },
			    Message = "token refreshed successfully",
			    Success = true
			    });
		    }
		    catch (Exception e)
		    {
			    return BadRequest(new GenericResponse<object> { Data = null, Message = e.Message, Success = false });
		    }
	    }
    }

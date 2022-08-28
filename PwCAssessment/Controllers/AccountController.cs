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
[Route("api/[controller]/[action]")][ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _accountService;

        public AccountController(IAuthService accountService)
        {
            _accountService = accountService;
        }
    
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<RegisterResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        
        [SwaggerOperation(Summary = "Create a user account")]
        public async Task<ActionResult<GenericResponse<RegisterResponse>>> CreateUser([FromBody] RegisterViewModel user)
        {
            var loggedUser = await _accountService.CreateUserAsync(user);
            return StatusCode((int)loggedUser.StatusCode, loggedUser);
        } 
        
     
    }

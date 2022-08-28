using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using PwCAssessment.Data;
using PwCAssessment.DTOs;
using PwCAssessment.DTOs.Requests;
using PwCAssessment.DTOs.Response;
using PwCAssessment.Interfaces;
using PwCAssessment.Models;

namespace PwCAssessment.Services;

       public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;

        public AuthService(SignInManager<ApplicationUser> signInManager,
	        ITokenService tokenService, ApplicationDbContext context, UserManager<ApplicationUser> userManager,
	        RoleManager<ApplicationRole> roleManager)
        {
            _signInManager = signInManager;
            _tokenService = tokenService;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
         public async Task<GenericResponse<RegisterResponse>> CreateUserAsync(RegisterViewModel model)
        {
            try
            {

                var user = new ApplicationUser()
                {

                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailConfirmed  = true


                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                  await  _context.UserRefreshTokens.AddAsync(new UserRefreshToken
                    {
                        Username = model.Email,
                        UserId = user.Id
                    });
                    await _context.SaveChangesAsync();


                    return new GenericResponse<RegisterResponse>
                    {
                        Data = new RegisterResponse { Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, UserId = user.Id },
                        Message = null,
                        Success = true,
                        StatusCode = HttpStatusCode.OK

                    };
                }
                else
                {
                    return new GenericResponse<RegisterResponse>
                    {
                        Data = null,
                        Message = result.Errors.FirstOrDefault()?.Description,
                        Success = false,
                        StatusCode = HttpStatusCode.BadRequest


                    };
                }
            }
            catch(Exception e)
            {
                return new GenericResponse<RegisterResponse>
                {
                    Data = null,
                    Message = e.InnerException?.ToString(),
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError


                };
            }
        }

             public async Task<GenericResponse<LoginResponse>> LoginAsync(LoginViewModel model)
	             {
	          
	             try
	             {
		             var user = await _userManager.FindByEmailAsync(model.Email);
      
		             if(user == null)
		             {
			             return new GenericResponse<LoginResponse>
			             {
				             Data = null,
				            // Message = $"user with Email: {model.Email} does not exist",
				             Message = "Incorrect Login Details",
				             Success = false,
				             StatusCode = HttpStatusCode.BadRequest


			             };

		             }

      
		             var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);
		             if (result.Succeeded)
		             {
			             var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
			            

			             var userClaims = new List<Claim>();
			             var claims = claimsPrincipal.Claims.ToList();

			             userClaims.AddRange(claims);
			             
			             var userRoles = await _userManager.GetRolesAsync(user);
			             foreach (var userRole in userRoles)
			             {
				             userClaims.Add(new Claim(ClaimTypes.Role, userRole));
				             var role = await _roleManager.FindByNameAsync(userRole);
				             if (role == null) continue;
				             var roleClaims = await _roleManager.GetClaimsAsync(role);
				             userClaims.AddRange(roleClaims);
			             }
			            

			             userClaims.AddRange(new List<Claim>
			             {
				             new(JwtRegisteredClaimNames.Sub, user.Email),
				             new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				             new(JwtRegisteredClaimNames.Email, user.Email),
				             new(ClaimTypes.Name, user.Email ?? "null"),
				          
                     
			             });

			             var token = _tokenService.GenerateAccessToken(userClaims);
			             var newRefreshToken = _tokenService.GenerateRefreshToken();

			             var userRefreshToken = await _tokenService.GetUserRefreshTokenByEmailAsync(user.Email);
			             if (userRefreshToken == null)
			             {
				             userRefreshToken =  new UserRefreshToken
				             {
					             Username = model.Email,
					             UserId = user.Id
				             };
				             await  _context.UserRefreshTokens.AddAsync(userRefreshToken);
				             await _context.SaveChangesAsync();
			             }

			             userRefreshToken.RefreshToken = newRefreshToken;
			             _context.UserRefreshTokens.Update(userRefreshToken);
			             await _context.SaveChangesAsync();


			             return new GenericResponse<LoginResponse>
			             {
				             Data = new LoginResponse { UserId = user.Id, Email = user.Email, RefreshToken = newRefreshToken, UserToken = token,  Firstname = user.FirstName, LastName = user.LastName},
				             Message = null,
				             Success = true,
				             StatusCode = HttpStatusCode.OK


			             };

		             }
		             else
		             {
			             return new GenericResponse<LoginResponse>
			             {
				             Data = null,
				             Message = "Incorrect Login Details",
				             Success = false,
				             StatusCode = HttpStatusCode.BadRequest


			             };
		             }
	             }
	             catch(Exception e)
	             {
		             return new GenericResponse<LoginResponse>
		             {
			             Data = null,
			             Message = e.Message,
			             Success = false,
			             StatusCode = HttpStatusCode.InternalServerError


		             };
	             }
	             }


 
    }

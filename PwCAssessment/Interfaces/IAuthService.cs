using PwCAssessment.DTOs;
using PwCAssessment.DTOs.Requests;
using PwCAssessment.DTOs.Response;

namespace PwCAssessment.Interfaces;

public interface IAuthService
{
    Task<GenericResponse<LoginResponse>> LoginAsync(LoginViewModel model);
    Task<GenericResponse<RegisterResponse>> CreateUserAsync(RegisterViewModel model);
}
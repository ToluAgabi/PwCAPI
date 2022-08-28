using PwCAssessment.DTOs;
using PwCAssessment.DTOs.Requests;
using PwCAssessment.DTOs.Response;

namespace PwCAssessment.Interfaces;

public interface ITravelService
{
    Task<GenericResponse<string>> SubmitTravelRequestAsync(TravelReqDto model);
    Task<GenericResponse<string>> UpdateTravelRequestAsync(string reqNumber, TravelReqDto requestDto);
    Task<GenericResponse<string>> DeleteTravelRequestAsync(string reqNumber);
    Task<GenericResponse<TravelResponseDto>> GetTravelRequestByReqNumberAsync(string reqNumber);
    Task<GenericResponse<List<TravelResponseDto>>> GetAllTravelRequestsAsync();
    Task<GenericResponse<List<SearchResponse>>> SearchLocationByName(string locationName);
}
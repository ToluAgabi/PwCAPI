using PwCAssessment.DTOs.Requests;

namespace PwCAssessment.DTOs.Response;

public class TravelResponseDto : TravelReqDto
{
    public string? RequisitionNumber { get; set; }

}
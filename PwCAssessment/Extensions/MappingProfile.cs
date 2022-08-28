using AutoMapper;
using PwCAssessment.DTOs.Requests;
using PwCAssessment.DTOs.Response;
using PwCAssessment.Models;

namespace PwCAssessment.Extensions;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<TravelReqDto, TravelRequest>();
		CreateMap<TravelRequest, TravelResponseDto>();
		CreateMap<TravelRequest, TravelReqDto>();
	}
}
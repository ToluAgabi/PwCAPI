using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PwCAssessment.Data;
using PwCAssessment.DTOs;
using PwCAssessment.DTOs.Requests;
using PwCAssessment.DTOs.Response;
using PwCAssessment.Interfaces;
using PwCAssessment.Models;
using PwCAssessment.Models.Enums;

namespace PwCAssessment.Services;

    public class TravelService : ITravelService
    {
        private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly HttpClient _client;
    

    public TravelService(ApplicationDbContext context, IMapper mapper, HttpClient client)
        {
            _context = context;
            _mapper = mapper;
            _client = client;
        }


    public async Task<GenericResponse<string>> SubmitTravelRequestAsync(TravelReqDto model)
    {
        try
        {
            var travelReq = _mapper.Map<TravelRequest>(model);
            _context.Add(travelReq);
            await _context.SaveChangesAsync();

            return new GenericResponse<string>
            {
                Success = true,
                Data = travelReq.RequisitionNumber,
                StatusCode = HttpStatusCode.OK,
                Message = "Request sent Successfully"
            };
        }
        catch
        {
            return new GenericResponse<string>
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An Error Occured"
            };
        }
    }

    public async Task<GenericResponse<string>> UpdateTravelRequestAsync(string reqNumber, TravelReqDto requestDto)
    {
        try
        {
            reqNumber = Uri.UnescapeDataString(reqNumber);

            var req = await _context.TravelRequests.SingleOrDefaultAsync(x => x.RequisitionNumber == reqNumber);

            if (req is null)
            {
                return new GenericResponse<string>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Request with given number does not exit"
                };
            }

            if (req.RequisitionStatus == RequisitionStatuses.Booked)
            {
                return new GenericResponse<string>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Request with given number is Booked and cannot be updated"
                };
            }

            _mapper.Map(requestDto, req);
            await _context.SaveChangesAsync();


            return new GenericResponse<string>
            {
                Success = true,
                Data = reqNumber,
                StatusCode = HttpStatusCode.OK,
                Message = "Request Updated Successfully"
            };
        }
        catch
        {
            return new GenericResponse<string>
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An Error Occured"
            };
        }
    }

    public async Task<GenericResponse<string>> DeleteTravelRequestAsync(string reqNumber)
        {
            try
            {
                reqNumber = Uri.UnescapeDataString(reqNumber);

                var req = await _context.TravelRequests.SingleOrDefaultAsync(x => x.RequisitionNumber == reqNumber);

                if (req is null)
                {
                    return new GenericResponse<string>
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Request with given number does not exit"
                    };
                }

                if (req.RequisitionStatus == RequisitionStatuses.Booked)
                {
                    return new GenericResponse<string>
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Request with given number is Booked and cannot be deleted"
                    };
                }

                _context.Remove(req);
                await _context.SaveChangesAsync();                
                
                
                return new GenericResponse<string>
                {
                    Success = true,
                    Data = reqNumber,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Request Deleted Successfully"
                };
            }
            catch
            {
                return new GenericResponse<string>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An Error Occured"
                };
            }


        }

    public async Task<GenericResponse<TravelResponseDto>> GetTravelRequestByReqNumberAsync(string reqNumber)
    {
        try
        {
            reqNumber = Uri.UnescapeDataString(reqNumber);

            var req = await _context.TravelRequests.SingleOrDefaultAsync(x => x.RequisitionNumber == reqNumber);

            if (req is null)
            {
                return new GenericResponse<TravelResponseDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Request with given number does not exit"
                };
            }

            var dto = new TravelResponseDto();

            _mapper.Map(req, dto);



            return new GenericResponse<TravelResponseDto>
            {
                Success = true,
                Data = dto,
                StatusCode = HttpStatusCode.OK,
            };
        }
        catch
        {
            return new GenericResponse<TravelResponseDto>
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An Error Occured"
            };
        }
    }

    public async Task<GenericResponse<List<TravelResponseDto>>> GetAllTravelRequestsAsync()
    {
        try
        {
            var req = await _context.TravelRequests.ToListAsync();


            var dto = new List<TravelResponseDto>();

            // var mappedRequests = 
            //       Mapper.Map<List<TravelRequest>, List<TravelReqDto>>(dto);

            _mapper.Map(req, dto);



            return new GenericResponse<List<TravelResponseDto>>
            {
                Success = true,
                Data = dto,
                StatusCode = HttpStatusCode.OK,
            };
        }
        catch
        {
            return new GenericResponse<List<TravelResponseDto>>
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An Error Occured"
            };
        }
    }

    public async Task<GenericResponse<List<SearchResponse>>> SearchLocationByName(string locationName)
        {
            try
            {
                locationName = Uri.UnescapeDataString(locationName);

                var searchResponse = new List<SearchResponse>();

                var url = $"https://restcountries.com/v3.1/name/{locationName}?fields=name,capital,capitalInfo,currencies";



                using var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    var des = JsonConvert.DeserializeObject<List<LocationResponse>>(result);
                    foreach (var item in des!)
                    {
                        var search = new SearchResponse
                        {
                            Country = item.Name?.Official,
                            Capital = item.Capital?.FirstOrDefault(),
                            Currencies = item.Currencies
                        };
                        
                        
                        var lat = des.FirstOrDefault()?.CapitalInfo?.Latlng?.FirstOrDefault();
                        var lng = des.FirstOrDefault()?.CapitalInfo?.Latlng?.LastOrDefault();
                        
                        var weatherUrl =
                            $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid=bc12083e70d2d22298c2df1cec7101d9";


                        using var weatherResponse = await _client.GetAsync(weatherUrl);

                        if (weatherResponse.IsSuccessStatusCode)
                        {
                            var weatherResult = await weatherResponse.Content.ReadAsStringAsync();

                            var weatherDes = JsonConvert.DeserializeObject<WeatherResponse>(weatherResult);
                            search.Weather = weatherDes?.Weather;
                            search.Climate = weatherDes?.Main;
                        }
                        searchResponse.Add(search);


                    }
                  

                }
                else
                {
                    return new GenericResponse<List<SearchResponse>>
                    {
                        Success = false,
                        Message = await response.Content.ReadAsStringAsync(),
                        StatusCode = response.StatusCode,
                    };
                }
                return new GenericResponse<List<SearchResponse>>
                {
                    Success = true,
                    Data = searchResponse,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            catch
            {
                return new GenericResponse<List<SearchResponse>>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An Error Occured"
                };
            }


        } 
       
    }

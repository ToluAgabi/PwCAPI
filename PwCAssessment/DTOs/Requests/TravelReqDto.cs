using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PwCAssessment.Extensions;
using PwCAssessment.Models.Enums;

namespace PwCAssessment.DTOs.Requests;

 public class TravelReqDto
 {
  

     [Required]
     public string? SourceLocation { get; set; }
     [Required]
     public string? SourceCountry { get; set; }
     [Required]
     public string? DestinationLocation { get; set; }
     [Required]
     public string? DestinationCountry { get; set; }
     [Required]
     public DateTimeOffset? DepartureTime { get; set; }

     
     [Required(ErrorMessage = "Please select a valid Travel Class, allowed: Economy, Business, FirstClass")]
     public TravelClasses? TravelClass { get; set; }
     [Required(ErrorMessage = "Please select a valid trip type, allowed: Oneway, RoundTrip")]
     public TripTypes? TripType { get; set; }
     [Required(ErrorMessage = "Please select a valid Requisition Status, allowed: Submitted, Approved,Booked, Canceled")]
     public RequisitionStatuses? RequisitionStatus { get; set; }
     [Required]
     public string? ChargeCode { get; set; }
     [Required]
     public string? TravellerName { get; set; }
     

 }


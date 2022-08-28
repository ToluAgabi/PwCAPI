using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PwCAssessment.Models.Enums;

namespace PwCAssessment.Models;

public class TravelRequest
{
    
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = null!;
    [Required]
    public DateTime? RequestDate { get; set; }
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
    [Required]
    public TravelClasses? TravelClass { get; set; }
    [Required]
    public TripTypes? TripType { get; set; }
    [Required]
    public string? ChargeCode { get; set; }
    [Required]
    public string? TravellerName { get; set; }
    [Required]
    public RequisitionStatuses? RequisitionStatus { get; set; }
    [Required]
    public string? RequisitionNumber { get; set; }
    

    public TravelRequest()
    {
        RequestDate = DateTime.Now;
    }

}
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using PwCAssessment.Data;

namespace PwCAssessment.Extensions;

public class CustomerNumberGen : ValueGenerator<string>
{

    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
    {
        if (entry == null)
        {
            throw new ArgumentNullException(nameof(entry));
        }
        var context = (ApplicationDbContext)entry.Context;

        

        var id = context.TravelRequests.FirstOrDefault()?.RequisitionNumber;
        if (id == null)
        {
            id = "Req" + "/PAS" + "/00001"; // Id Format
        }
        else
            //Regex to generate Id
        {
      
         var customerNumber = context.TravelRequests.Select(x => x.RequisitionNumber).ToList();


         var ordered = customerNumber.Select(s => new { Str = s, Split = s.Split('/') })
                .OrderBy(x => int.Parse(x.Split[2]))
                .Select(x => x.Str)
                .ToList();
            id = Regex.Replace(ordered.LastOrDefault() ?? throw new InvalidOperationException(),
                @"(?<=(\D|^))\d+(?=\D*$)", m => (int.Parse(m.Value) + 1)
                    .ToString(new string('0', m.Value.Length)));

        }

        return id;

    }
}
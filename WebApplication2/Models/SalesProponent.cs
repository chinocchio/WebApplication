
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class SalesProponent
    {
        public int SalesProponentId { get; set; }
        public string? Broker { get; set; }
        public string? PS_QC_ISM { get; set; }
        public string? MarketingOfficer { get; set; }
        public string? MarketingManager { get; set; }
        public string? DeputyMarketingDirector { get; set; }
        public string? MarketingDirector { get; set; }
        public List<SalesTransaction>? SalesTransaction { get; set; }

        [NotMapped]
        public string DisplayText { get; set; } = string.Empty;
    }
}

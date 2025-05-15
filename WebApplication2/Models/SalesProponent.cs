using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    [Keyless]
    public class SalesProponent
    {
        public string? Roles { get; set; }
        public long ProponentBpNumber { get; set; }
        public string? Fullname { get; set; }
        public string? ReportingTo { get; set; }
        public List<SalesTransaction>? SalesTransaction { get; set; }
    }
}

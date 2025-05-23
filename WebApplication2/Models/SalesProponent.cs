using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class SalesProponent
    {
        [Key]
        public long ProponentBpNumber { get; set; }
        public string? Roles { get; set; }
        public string? Fullname { get; set; }
        public string? ReportingTo { get; set; }
    }
}

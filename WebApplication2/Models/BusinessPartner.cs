using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class BusinessPartner
    {
        public int BusinessPartnerId { get; set; }
        public string? Role { get; set; } = string.Empty;
        public string? Fullname { get; set; } = string.Empty;
        public string? CustomerCode { get; set; }
        public string? ClientBase { get; set; } = string.Empty;
        public string? IdSubmitted { get; set; } = string.Empty;
        public DateOnly? IdDateSubmitted { get; set; }
        public string? EmailAddress { get; set; } = string.Empty;
        public string? ContactNumber { get; set; } = string.Empty;
        public List<SalesTransaction>? SalesTransaction { get; set; }
        
        [NotMapped] // Prevent EF from mapping to DB
        public List<BusinessPartner>? OtherBuyers { get; set; }
    }
}

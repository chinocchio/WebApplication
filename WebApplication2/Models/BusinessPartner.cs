namespace WebApplication2.Models
{
    public class BusinessPartner
    {
        public int BusinessPartnerId { get; set; }
        public string? Role { get; set; } = string.Empty;
        public string? Fullname { get; set; } = string.Empty;
        public int? CustomerCode { get; set; }
        public List<SalesTransaction>? SalesTransaction { get; set; }
    }
}

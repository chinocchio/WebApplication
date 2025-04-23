using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebApplication2.Models
{
    public class SalesTransaction
    {
        public int SalesTransactionId { get; set; }
        public long ContractNumber { get; set; }
        public string? TypeOfSale { get; set; } = string.Empty;
        public int? BusinessPartnerId { get; set; }
        public int? PropertyId { get; set; }
        public int? SalesProponentsId { get; set; }
        public Property? Properties { get; set; }
        public BusinessPartner? BusinessPartner { get; set; }
        public SalesProponent? SalesProponent { get; set; }
    }
}

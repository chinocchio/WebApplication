using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebApplication2.Models
{
    public class SalesTransaction
    {
        public int SalesTransactionId { get; set; }
        public long ContractNumber { get; set; }
        public string? TypeOfSale { get; set; } = string.Empty;
        public DateOnly HoldingDate { get; set; }
        public string? TransactionType { get; set; } = string.Empty;
        public string? PromoDiscount { get; set; } = string.Empty;
        public string? StatusInGeneral { get; set; } = string.Empty;
        public string? Milestone { get; set; } = string.Empty;
        public string? NewColorStatus { get; set; } = string.Empty;
        
        public int? BusinessPartnerId { get; set; }
        public int? PropertyId { get; set; }
        public int? SalesProponentsId { get; set; }
        public int? ReservationFeeId { get; set; }
        public Property? Properties { get; set; }
        public BusinessPartner? BusinessPartner { get; set; }
        public SalesProponent? SalesProponent { get; set; }
        public ReservationFee? ReservationFee { get; set; }
    }
}

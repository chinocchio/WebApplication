namespace WebApplication2.Models
{
    public class BuyerLedger
    {
        public int Id { get; set; }
        public long? ContractNumber { get; set; }
        public string? CustomerCode { get; set; }
        public string? UnitCode { get; set; }
        public DateOnly? PaymentTermSchedule { get; set; }
        public string? PaymentNumber { get; set; }
        public double? AmountDue { get; set; }
        public DateOnly? WhenDue { get; set; }
        public string? PaymentReferenceDocNumber { get; set; }
        public string? PaymentReferenceDocType { get; set; }
        public string? PaymentReferenceAmount { get; set; }
        public DateOnly? PaymentReferenceDate { get; set; }
    }
}

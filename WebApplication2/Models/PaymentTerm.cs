using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class PaymentTerm // change name to payment term
    {
        public int PaymentTermId { get; set; }
        public DateOnly? RfDatePaid { get; set; }
        public double? RfAmountPaidToUnit { get; set; }
        public double? RfAmountPaidToGMTOE { get; set; }
        public double? RfAmountGMTOE_Unit { get; set; }
        public DateOnly? RfDateCredited { get; set; }
        public string? RfOrNumber { get; set; } = null!;
        public string? Paymentterm { get; set; }
        public string? PercentTOB { get; set; }
        public string? TOBModeOfPayment { get; set; }
        public double? TOB { get; set; }
        public double? EstimatedBankMAFor7Point5Percent { get; set; }
        public DateOnly? StartDate1stMA { get; set; }
        public double? UnitParking { get; set; }
        public double? TFee { get; set; }
        public double? AmountPaidToUnit { get; set; }
        public double? AmountPaidToTF { get; set; }
        public DateOnly? DatePaid { get; set; }
        public string? FirstMAOrNumber { get; set; }
        public int? PaymentDueDate { get; set; }
        public double? Unit { get; set; }
        public double? TransferFee { get; set; }
        public double? Total { get; set; }
        public string? PercentPayment { get; set; }
        public string? PaymentCategory { get; set; }
        public List<SalesTransaction>? SalesTransaction { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class ReservationFee
    {
        public int ReservationFeeId { get; set; }
        public DateOnly RfDatePaid { get; set; }
        //public string RfProofUsedUponHolding { get; set; } = null!; // Ilipat sa SalesDocument
        public double RfAmountPaidToUnit { get; set; }
        public double RfAmountPaidToGMTOE { get; set; }
        public double RfAmountGMTOE_Unit { get; set; }
        public DateOnly RfDateCredited { get; set; }
        public string RfOrNumber { get; set; } = null!;
        public SalesTransaction SalesTransaction { get; set; } = null!;
    }
}

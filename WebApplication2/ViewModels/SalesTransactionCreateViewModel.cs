
using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class SalesTransactionCreateViewModel
    {
        // SalesTransaction Properties
        public int SalesTransactionId { get; set; }

        [Required]
        public long ContractNumber { get; set; }

        [Required]
        public string TypeOfSale { get; set; } = string.Empty;

        [Required]
        public DateOnly HoldingDate { get; set; }

        [Required]
        public string TransactionType { get; set; } = string.Empty;

        public string PromoDiscount { get; set; } = string.Empty;
        public string StatusInGeneral { get; set; } = string.Empty;
        public string Milestone { get; set; } = string.Empty;
        public string NewColorStatus { get; set; } = string.Empty;

        // Foreign Keys
        public int? PropertyId { get; set; }
        public int? SalesProponentsId { get; set; }
        public int? ReservationFeeId { get; set; }
        public int? BusinessPartnerId { get; set; }

        // Properties for selecting existing entities via dropdowns
        public List<BusinessPartner> AvailableBusinessPartners { get; set; }
        public List<SalesProponent> AvailableSalesProponents { get; set; }
        public List<ReservationFee> AvailableReservationFees { get; set; }
        public List<Property> AvailableProperties { get; set; }

        // For multiple buyers with the same CustomerCode
        public List<int> SelectedBusinessPartnerIds { get; set; } = new List<int>();

        // Properties for creating new records
        public string NewBusinessPartnerFullName { get; set; } = string.Empty;
        public long? NewCustomerCode { get; set; }
        public string NewEmailAddress { get; set; } = string.Empty;
        public string NewContactNumber { get; set; } = string.Empty;

        public DateOnly? NewReservationFeeDatePaid { get; set; }
        public double? NewReservationFeeAmountPaid { get; set; }
        public string NewReservationFeeOrNumber { get; set; } = string.Empty;

        public string NewSalesProponentBroker { get; set; } = string.Empty;
        public string NewSalesProponentPSQCISM { get; set; } = string.Empty;
        public string NewSalesProponentMarketingManager { get; set; } = string.Empty;
        public string NewSalesProponentDeputyMarketingDirector { get; set; } = string.Empty;
        public string NewSalesProponentMarketingDirector { get; set; } = string.Empty;
        public string NewSalesProponentMarketingOfficer { get; set; } = string.Empty;
    }
}

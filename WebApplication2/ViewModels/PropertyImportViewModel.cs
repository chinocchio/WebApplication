using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class PropertyImportViewModel
    {
        [Required(ErrorMessage = "Please select a file")]
        public IFormFile? File { get; set; }

        public List<string>? ImportErrors { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
    }

    public class PropertyImportRow
    {
        // Property Information
        public string PropertyType { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string BuildingPhase { get; set; } = string.Empty;
        public string UnitCode { get; set; } = string.Empty;

        // Sales Transaction Information
        public long ContractNumber { get; set; }
        public string? TypeOfSale { get; set; }
        public DateOnly HoldingDate { get; set; }
        public string? TransactionType { get; set; }
        public string? PromoDiscount { get; set; }
        public string? StatusInGeneral { get; set; }
        public string? Milestone { get; set; }
        public string? NewColorStatus { get; set; }

        // Business Partner Information (Principal Buyer)
        public string CustomerCode { get; set; } = string.Empty;
    }
} 
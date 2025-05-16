using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class SalesTransactionImportViewModel
    {
        [Required(ErrorMessage = "Please select a file")]
        public IFormFile? File { get; set; }

        public List<string>? ImportErrors { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
    }

    public class SalesTransactionImportRow
    {
        public long ContractNumber { get; set; }
        public string? TypeOfSale { get; set; }
        public DateOnly HoldingDate { get; set; }
        public string? TransactionType { get; set; }
        public string? StatusInGeneral { get; set; }
        public string? Milestone { get; set; }
        public string? NewColorStatus { get; set; }
    }
} 
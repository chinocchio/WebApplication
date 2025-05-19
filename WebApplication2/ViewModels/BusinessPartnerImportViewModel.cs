using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class BusinessPartnerImportViewModel
    {
        [Required(ErrorMessage = "Please select a file")]
        public IFormFile? File { get; set; }

        public List<string>? ImportErrors { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
    }

    public class BusinessPartnerImportRow
    {
        public string? CustomerCode { get; set; }
        public string? Role { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public string? ClientBase { get; set; }
        public string? IdSubmitted { get; set; }
        public DateTime? IdDateSubmitted { get; set; }
        public string? EmailAddress { get; set; }
        public string? ContactNumber { get; set; }
        public long ContractNumber { get; set; } // This will be used to link with SalesTransaction
    }
} 
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class PaymentTermImportViewModel
    {
        [Required(ErrorMessage = "Please select a file to import")]
        [Display(Name = "Excel File")]
        public IFormFile File { get; set; }

        public List<string> ImportErrors { get; set; } = new List<string>();
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
    }
} 
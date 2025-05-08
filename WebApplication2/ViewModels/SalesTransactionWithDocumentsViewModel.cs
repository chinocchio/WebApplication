using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class SalesTransactionWithDocumentsViewModel
    {
        public SalesTransaction? SalesTransaction { get; set; }
        public List<DocumentSubmitted> SubmittedDocuments { get; set; } = new(); // Match by ContractNumber
        public List<DocumentForSubmission> DocumentsForSubmission { get; set; } = new();
        public List<BuyerLedger> BuyerLedgers { get; set; } = new();
    }
}


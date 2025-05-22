using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class SalesTransactionWithDocumentsViewModel
    {
        public SalesTransaction? SalesTransaction { get; set; }
        public List<SubmittedDocument> SubmittedDocuments { get; set; } = new(); // Match by ContractNumber
        public List<DocumentForSubmission> DocumentsForSubmission { get; set; } = new();
        public List<BuyerLedger> BuyerLedgers { get; set; } = new();
        public SalesProponent? SalesProponent { get; set; }
        public List<SalesProponent> ProponentHierarchy { get; set; } = new(); // Store the reporting hierarchy
    }
}


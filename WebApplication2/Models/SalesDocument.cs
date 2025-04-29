namespace WebApplication2.Models
{
    public class SalesDocument
    {
        public int SalesDocumentId { get; set; }
        public string RfProofUsedUponHolding { get; set; } = null!;
        public DateOnly Sa1Sa2CompletionDate { get; set; }
        public string? ProofOfIncomeSubmitted { get; set; }
        public DateOnly DocsSubmissionForCiEndorsement { get; set; }
        public string? TotalPDCSRequired { get; set; }
        public string? BankPDCIssued { get; set; }
        public string? TotalAdaPdcsSubmitted { get; set; }
        public DateOnly DateSubmitted { get; set; }
        public DateOnly PDCCompletionDate { get; set; }
        public DateOnly CTSToDate { get; set; }
        public string? CTSStatus { get; set; }
        public string? TypeOfCTS { get; set; }
        public string? SignedByBroker { get; set; }
        public string? CtsACN { get; set; }
        public DateOnly DateRecievedFromSalesAdmin { get; set; }
        public string? DateNotary { get; set; }
        public DateOnly SetBDocsCompletionDate { get; set; }
        public DateOnly ContractedSaleDate { get; set; }
        public string? MonthContracted { get; set; }
        public string? MonthBooked { get; set; }
        public string? P1OldColorStatus { get; set; }
        public string? AegingCategory { get; set; }
        public string? LackingDocs { get; set; }
        public DateOnly CtsSpaAndDOROutDate { get; set; }
        public DateOnly ComplianceDUe { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public List<SalesTransaction>? SalesTransaction { get; set; }
    }
}

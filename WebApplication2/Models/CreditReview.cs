namespace WebApplication2.Models
{
    public class CreditReview
    {
        public int CreditReviewId { get; set; }
        public string? CreditReviewResult { get; set; }
        public string? CMAPResult { get; set; }
        public string? CreditReviewRemarks { get; set; }
        public string? NDIStatus { get; set; }
        public string? IsBankApporvable  { get; set; }
        public string? RedTag { get; set; }
        public string? RedTagReason { get; set; }
        public DateOnly CiCompletionDate { get; set; }
        public string? TypeOfIncome { get; set; }
        public string? NdiRate { get; set; }
        public int NetDisposableIncome { get; set; }
        public int OtherLoans { get; set; }
        public int NetNdi { get; set; }
        public int NdiVsBankMaTobAmt { get; set; }
        public string? PercentOfNdiVsMa { get; set; }
        public string? NdiCategory { get; set; }
        public int MaxTerm { get; set; }
        public string? EstimatedMaxTerm { get; set; }
        public List<SalesTransaction>? SalesTransaction { get; set; }
    }
}

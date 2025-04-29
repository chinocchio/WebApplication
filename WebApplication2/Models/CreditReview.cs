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
        public string? ReasonForPurchase { get; set; }
        public string? FirstHomeInPH { get; set; }
        public int? NumberOfHomesInPH { get; set; }
        public string? WithOtherCPGIUnits { get; set; }
        public string? ProjectOrUnitCodeOfOtherCPGIUnit { get; set; }
        public double? IncomeDeclaredPb { get; set; }
        public double? IncomeDeclaredCb { get; set; }
        public double? TotalIncomeCombined { get; set; }
        public string? TypeOfIncome { get; set; }
        public string? NdiRate { get; set; }
        public double? NetDisposableIncome { get; set; }
        public double? OtherLoans { get; set; }
        public double? NetNdi { get; set; }
        public double? NdiVsBankMaTobAmt { get; set; }
        public string? PercentOfNdiVsMa { get; set; }
        public string? NdiCategory { get; set; }
        public int MaxTerm { get; set; }
        public string? EstimatedMaxTerm { get; set; }
        public string? PersonaCategory { get; set; }
        public string? ImmigrantOrNonImmigrant { get; set; }
        public string? HighRishk { get; set; }
        public string? HighRiskFactors { get; set; }
        public List<SalesTransaction>? SalesTransaction { get; set; }
    }
}

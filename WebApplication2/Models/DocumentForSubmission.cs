namespace WebApplication2.Models
{
    public class DocumentForSubmission
    {
        public int Id { get; set; }
        public string? DocumentCode { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentFinanceType { get; set; }
        public string? DocumentLocation { get; set; }
        public string? DocumentSourceOfIncome { get; set; }
        public string? DocumentGroup { get; set; }
    }
}

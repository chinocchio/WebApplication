namespace WebApplication2.Models
{
    public class DocumentRegistry
    {
        public int Id { get; set; }
        public string? DocumentCode { get; set; }
        public string? Description { get; set; }
        public string? DoesExprire { get; set; }
        public int? DurationInMonths { get; set; }
    }
}

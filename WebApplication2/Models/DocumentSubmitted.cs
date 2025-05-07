namespace WebApplication2.Models
{
    public class DocumentSubmitted
    {
        public int Id { get; set; }
        public string? DocumentCode { get; set; }
        public string? DocumentName { get; set; }
        public DateOnly? DateSubmitted { get; set; }
        public long? ContractNumber { get; set; }
        public string? CustomerCode { get; set; }
        public string? UnitCode { get; set; }


    }
}

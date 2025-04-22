namespace WebApplication2.Models
{
    public class Property
    {
        public int PropertyId { get; set; }
        public string? PropertyType { get; set; } = string.Empty;
        public string? ProjectName { get; set; } = string.Empty;
        public string? BuildingPhase { get; set; } = string.Empty;
        public string? UnitCode { get; set; } = string.Empty;
        public List<SalesTransaction>? SalesTransaction { get; set; } 
    }
}

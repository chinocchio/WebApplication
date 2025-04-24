using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Property
    {
        public int PropertyId { get; set; }
        [Required(ErrorMessage = "Property Type is required.")]
        public string PropertyType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Project Name is required.")]
        public string ProjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Building Phase is required.")]
        public string BuildingPhase { get; set; } = string.Empty;

        [Required(ErrorMessage = "Unit Code is required.")]
        public string UnitCode { get; set; } = string.Empty;
        public List<SalesTransaction>? SalesTransaction { get; set; }
        [NotMapped]
        public string DisplayText { get; set; } = string.Empty;
    }
}

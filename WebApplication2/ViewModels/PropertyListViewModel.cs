using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class PropertyListViewModel
    {
        public IEnumerable<Property>? Properties { get; set; }

        public string? SearchTerm { get; set; }
    }
}

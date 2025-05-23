using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class SalesProponentViewModel
    {
        public long ContractNumber { get; set; }
        public List<SelectListItem>? AvailableProponents { get; set; }
        public long? SelectedProponentBpNumber { get; set; }
        public List<SalesProponent>? CurrentProponents { get; set; }
        public string? SelectedRole { get; set; }
        public long? NewBpNumber { get; set; }
        public string? NewFullname { get; set; }
        public string? NewReportingTo { get; set; }

        public List<SelectListItem> Roles { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Broker", Text = "Broker" },
            new SelectListItem { Value = "PS/QC/ISM", Text = "PS/QC/ISM" },
            new SelectListItem { Value = "Marketing Officer", Text = "Marketing Officer" },
            new SelectListItem { Value = "Marketing Manager", Text = "Marketing Manager" },
            new SelectListItem { Value = "Deputy Marketing Director", Text = "Deputy Marketing Director" },
            new SelectListItem { Value = "Marketing Director", Text = "Marketing Director" }
        };
    }
} 
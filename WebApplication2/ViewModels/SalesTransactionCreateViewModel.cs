﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class SalesTransactionCreateViewModel
    {

        // Dropdown selection
        public int? SelectedBusinessPartnerId { get; set; }
        public List<SelectListItem>? ExistingBusinessPartners { get; set; }
        public int SalesTransactionId { get; set; }
        // New Business Partner fields (if not selecting existing one)
        public string? Role { get; set; }
        public string? Fullname { get; set; }
        public string? CustomerCode { get; set; }
        public string? ClientBase { get; set; }
        public string? IdSubmitted { get; set; }
        public DateOnly? IdDateSubmitted { get; set; }
        public string? EmailAddress { get; set; }
        public string? ContactNumber { get; set; }

        // For selecting a property
        public string? SelectedProjectName { get; set; }
        public string? SelectedBuildingPhase { get; set; }
        public int? SelectedPropertyId { get; set; }
        public List<SelectListItem>? ProjectNames { get; set; }
        public List<SelectListItem>? BuildingPhases { get; set; }
        public List<SelectListItem>? Properties { get; set; }

        // For selecting a sales proponent
        public long? SelectedProponentBpNumber { get; set; }
        public List<SelectListItem>? SalesProponents { get; set; }

        // SalesTransaction Fields
        public long ContractNumber { get; set; }
        public string? TypeOfSale { get; set; }
        public DateOnly HoldingDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string? TransactionType { get; set; }
        public string? PromoDiscount { get; set; }
        public string? StatusInGeneral { get; set; }
        public string? Milestone { get; set; }
        public string? NewColorStatus { get; set; }
    }
}

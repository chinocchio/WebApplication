using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class SalesTransactionController : Controller
    {
        private readonly DataContext _context;

        public SalesTransactionController(DataContext context)
        {
            _context = context;
        }

        // GET: SalesTransaction/Create
        public IActionResult Create()
        {
            var model = new SalesTransactionCreateViewModel
            {
                Properties = _context.Properties
                .Select(p => new SelectListItem
                {
                    Value = p.PropertyId.ToString(),
                    Text = $"{p.ProjectName} - {p.BuildingPhase} - {p.UnitCode}"
                }).ToList(),

                ExistingBusinessPartners = _context.BusinessPartners.Select(bp => new SelectListItem
                {
                    Value = bp.BusinessPartnerId.ToString(),
                    Text = $"{bp.Role } - {bp.Fullname} - {bp.CustomerCode}"
                }).ToList()
            };

            return View(model);
        }

        // POST: SalesTransaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SalesTransactionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessPartner businessPartner;

                if (model.SelectedBusinessPartnerId.HasValue)
                {
                    // Use selected existing partner
                    businessPartner = _context.BusinessPartners
                        .First(bp => bp.BusinessPartnerId == model.SelectedBusinessPartnerId.Value);
                }
                else
                {
                    // Create new one
                    businessPartner = new BusinessPartner
                    {
                        Role = model.Role,
                        Fullname = model.Fullname,
                        CustomerCode = model.CustomerCode?.ToString(),
                        ClientBase = model.ClientBase,
                        IdSubmitted = model.IdSubmitted,
                        IdDateSubmitted = model.IdDateSubmitted,
                        EmailAddress = model.EmailAddress,
                        ContactNumber = model.ContactNumber
                    };

                    _context.BusinessPartners.Add(businessPartner);
                    _context.SaveChanges();
                }

                // Optionally: Create a new SalesTransaction to link to the selected property
                if (model.SelectedPropertyId.HasValue)
                {
                    var transaction = new SalesTransaction
                    {
                        ContractNumber = model.ContractNumber,
                        TypeOfSale = model.TypeOfSale,
                        TransactionType = model.TransactionType,
                        PromoDiscount = model.PromoDiscount,
                        StatusInGeneral = model.StatusInGeneral,
                        Milestone = model.Milestone,
                        NewColorStatus = model.NewColorStatus,
                        BusinessPartnerId = businessPartner.BusinessPartnerId,
                        PropertyId = model.SelectedPropertyId.Value,
                        HoldingDate = DateOnly.FromDateTime(DateTime.Now)
                    };

                    _context.SalesTransactions.Add(transaction);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            // Repopulate Properties if ModelState failed
            model.Properties = _context.Properties
                .Select(p => new SelectListItem
                {
                    Value = p.PropertyId.ToString(),
                    Text = $"{p.ProjectName} - {p.BuildingPhase} - {p.UnitCode}"
                }).ToList();

            model.ExistingBusinessPartners = _context.BusinessPartners.Select(bp => new SelectListItem
            {
                Value = bp.BusinessPartnerId.ToString(),
                Text = $"{bp.Fullname} - {bp.CustomerCode}"
            }).ToList();

            return View(model);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    // Controller to ng mga sales transaction, boss. Dito lahat ng galawan ng benta. Wag mo gagalawin ng basta-basta, baka magka-leche-leche ang sales mo!
    public class SalesTransactionController : Controller
    {
        private readonly DataContext _context;

        // Konstruktor to, boss. Dito pinapasa yung database context. Parang suplay ng kuryente, wag mo puputulin!
        public SalesTransactionController(DataContext context)
        {
            _context = context;
        }

        // GET: SalesTransaction/Create
        // Gawa ng bagong benta, parang nagbebenta ka ng yaman. Dito ka mag-ingat, boss!
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
        // Eto na yung tunay na laban, boss. Dito na isasave yung bagong benta mo. Wag kang magkamali dito!
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SalesTransactionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessPartner businessPartner;

                if (model.SelectedBusinessPartnerId.HasValue)
                {
                    // Gamitin yung existing na partner. Wag ka na gumawa ng bago kung meron na.
                    businessPartner = _context.BusinessPartners
                        .First(bp => bp.BusinessPartnerId == model.SelectedBusinessPartnerId.Value);
                }
                else
                {
                    // Gawa ng bagong business partner kung wala pa. Parang bagong tropa sa barkada.
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

                // Gawa ng bagong SalesTransaction na naka-link sa property. Dito na ang totoong transaksyon!
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

                // Pag successful, balik sa listahan ng benta. Parang tapos na ang laban, boss!
                return RedirectToAction("Index");
            }

            // Pag sablay, ulitin dropdowns. Wag kang tamad, ayusin mo input mo!
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

        // Boss, wag mo gagalawin to kung di mo alam ginagawa mo. Delikado 'to!
        private bool SalesTransactionExists(int id)
        {
            return _context.SalesTransactions.Any(e => e.SalesTransactionId == id);
        }
    }
}

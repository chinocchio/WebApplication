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
            // Get all unique project names
            var projectNames = _context.Properties
                .AsNoTracking()  // For better performance
                .Select(p => p.ProjectName)
                .Distinct()
                .OrderBy(p => p)  // Sort alphabetically
                .ToList();

            var model = new SalesTransactionCreateViewModel
            {
                ProjectNames = projectNames.Select(p => new SelectListItem
                {
                    Value = p,
                    Text = p
                }).ToList(),

                ExistingBusinessPartners = _context.BusinessPartners
                    .AsNoTracking()
                    .Select(bp => new SelectListItem
                    {
                        Value = bp.BusinessPartnerId.ToString(),
                        Text = $"{bp.Role } - {bp.Fullname} - {bp.CustomerCode}"
                    }).ToList(),

                SalesProponents = _context.SalesProponents
                    .AsNoTracking()
                    .Select(sp => new SelectListItem
                    {
                        Value = sp.ProponentBpNumber.ToString(),
                        Text = $"{sp.Roles} - {sp.Fullname} - {sp.ProponentBpNumber}"
                    }).ToList()
            };

            // For debugging - check if project names are loaded
            System.Diagnostics.Debug.WriteLine($"Loaded {projectNames.Count} project names");
            foreach (var name in projectNames)
            {
                System.Diagnostics.Debug.WriteLine($"Project Name: {name}");
            }

            return View(model);
        }

        // GET: SalesTransaction/GetBuildingPhases
        [HttpGet]
        public IActionResult GetBuildingPhases(string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                return BadRequest("Project name is required");
            }

            var phases = _context.Properties
                .AsNoTracking()
                .Where(p => p.ProjectName == projectName)
                .Select(p => p.BuildingPhase)
                .Distinct()
                .OrderBy(p => p)  // Sort alphabetically
                .ToList();

            // For debugging - check if phases are found
            System.Diagnostics.Debug.WriteLine($"Found {phases.Count} phases for project {projectName}");
            foreach (var phase in phases)
            {
                System.Diagnostics.Debug.WriteLine($"Phase: {phase}");
            }

            return Json(phases);
        }

        // GET: SalesTransaction/GetAvailableUnits
        [HttpGet]
        public IActionResult GetAvailableUnits(string projectName, string buildingPhase)
        {
            if (string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(buildingPhase))
            {
                return BadRequest("Both project name and building phase are required");
            }

            var units = _context.Properties
                .AsNoTracking()
                .Where(p => p.ProjectName == projectName && p.BuildingPhase == buildingPhase)
                .Select(p => new
                {
                    propertyId = p.PropertyId,
                    unitCode = p.UnitCode
                })
                .OrderBy(p => p.unitCode)  // Sort alphabetically
                .ToList();

            // For debugging - check if units are found
            System.Diagnostics.Debug.WriteLine($"Found {units.Count} units for project {projectName}, phase {buildingPhase}");
            foreach (var unit in units)
            {
                System.Diagnostics.Debug.WriteLine($"Unit: {unit.unitCode} (ID: {unit.propertyId})");
            }

            return Json(units);
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
                        ProponentBpNumber = model.SelectedProponentBpNumber,
                        HoldingDate = DateOnly.FromDateTime(DateTime.Now)
                    };

                    _context.SalesTransactions.Add(transaction);
                    _context.SaveChanges();
                }

                // Pag successful, balik sa listahan ng benta. Parang tapos na ang laban, boss!
                return RedirectToAction("Index");
            }

            // Repopulate dropdowns if validation fails
            model.ProjectNames = _context.Properties
                .Select(p => p.ProjectName)
                .Distinct()
                .Select(p => new SelectListItem
                {
                    Value = p,
                    Text = p
                }).ToList();

            if (!string.IsNullOrEmpty(model.SelectedProjectName))
            {
                model.BuildingPhases = _context.Properties
                    .Where(p => p.ProjectName == model.SelectedProjectName)
                    .Select(p => p.BuildingPhase)
                    .Distinct()
                    .Select(p => new SelectListItem
                    {
                        Value = p,
                        Text = p
                    }).ToList();
            }

            if (!string.IsNullOrEmpty(model.SelectedProjectName) && !string.IsNullOrEmpty(model.SelectedBuildingPhase))
            {
                model.Properties = _context.Properties
                    .Where(p => p.ProjectName == model.SelectedProjectName && p.BuildingPhase == model.SelectedBuildingPhase)
                    .Select(p => new SelectListItem
                    {
                        Value = p.PropertyId.ToString(),
                        Text = p.UnitCode
                    }).ToList();
            }

            model.ExistingBusinessPartners = _context.BusinessPartners.Select(bp => new SelectListItem
            {
                Value = bp.BusinessPartnerId.ToString(),
                Text = $"{bp.Fullname} - {bp.CustomerCode}"
            }).ToList();

            model.SalesProponents = _context.SalesProponents.Select(sp => new SelectListItem
            {
                Value = sp.ProponentBpNumber.ToString(),
                Text = $"{sp.Roles} - {sp.Fullname} - {sp.ProponentBpNumber}"
            }).ToList();

            return View(model);
        }

        // Boss, wag mo gagalawin to kung di mo alam ginagawa mo. Delikado 'to!
        private bool SalesTransactionExists(int id)
        {
            return _context.SalesTransactions.Any(e => e.SalesTransactionId == id);
        }

        //GET: Property/SearchResults
        public async Task<IActionResult> SearchResults(string? searchTerm)
        {
            ViewBag.SearchTerm = searchTerm;

            var query = _context.SalesTransactions
                        .AsNoTracking()
                        .Include(st => st.Properties)
                        .Include(st => st.BusinessPartner)
                        .Include(st => st.PaymentTerm)
                        .Include(st => st.CreditReview)
                        .Include(st => st.SalesDocument)
                        .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(st =>
                    st.ContractNumber.ToString().Contains(searchTerm) ||
                    st.BusinessPartner.Fullname.ToLower().Contains(searchTerm) ||
                    st.BusinessPartner.CustomerCode.ToString().Contains(searchTerm) ||
                    st.Properties.UnitCode.ToLower().Contains(searchTerm) ||
                    st.ProponentBpNumber.ToString().Contains(searchTerm)
                );
            }

            query = query.OrderByDescending(st => st.HoldingDate);

            var salesTransactions = await query.ToListAsync();

            var submittedDocuments = await _context.SubmittedDocuments
                .AsNoTracking()
                .ToListAsync();

            var allDocumentsForSubmission = await _context.DocumentForSubmissions
                .AsNoTracking()
                .ToListAsync();

            // Get all unique ProponentBpNumbers from the transactions
            var proponentBpNumbers = salesTransactions
                .Where(st => st.ProponentBpNumber.HasValue)
                .Select(st => st.ProponentBpNumber.Value)
                .Distinct()
                .ToList();

            // Fetch all SalesProponents in one query (we need all of them for the hierarchy)
            var allSalesProponents = await _context.SalesProponents
                .AsNoTracking()
                .ToDictionaryAsync(sp => sp.ProponentBpNumber);

            var result = salesTransactions.Select(st => {
                var submittedForThis = submittedDocuments
                    .Where(doc => doc.ContractNumber == st.ContractNumber)
                    .ToList();

                var submittedDocCodes = submittedForThis.Select(doc => doc.DocumentCode).ToHashSet();

                var remainingRequired = allDocumentsForSubmission
                    .Where(doc =>!submittedDocCodes.Contains(doc.DocumentCode))
                    .ToList();

                var matchingLedgers = _context.BuyerLedgers
                   .AsNoTracking()
                   .Where(bl => bl.ContractNumber == st.ContractNumber)
                   .ToList();

                // Get the initial proponent
                allSalesProponents.TryGetValue(st.ProponentBpNumber ?? 0, out var initialProponent);

                // Build the reporting hierarchy
                var proponentHierarchy = new List<SalesProponent>();
                if (initialProponent != null)
                {
                    var currentProponent = initialProponent;
                    proponentHierarchy.Add(currentProponent);

                    // Follow the reporting chain until we reach someone with no reporting to
                    while (!string.IsNullOrEmpty(currentProponent.ReportingTo))
                    {
                        // Try to find the next person in the reporting chain
                        var reportingToNumber = long.TryParse(currentProponent.ReportingTo, out var number) ? number : 0;
                        if (reportingToNumber > 0 && allSalesProponents.TryGetValue(reportingToNumber, out var nextProponent))
                        {
                            proponentHierarchy.Add(nextProponent);
                            currentProponent = nextProponent;
                        }
                        else
                        {
                            // Break if we can't find the next person
                            break;
                        }
                    }
                }

                return new SalesTransactionWithDocumentsViewModel
                {
                    SalesTransaction = st,
                    SubmittedDocuments = submittedForThis,
                    DocumentsForSubmission = remainingRequired,
                    BuyerLedgers = matchingLedgers,
                    SalesProponent = initialProponent,
                    ProponentHierarchy = proponentHierarchy
                };
            }).ToList();

            foreach (var item in result)
            {
                var transaction = item.SalesTransaction;

                if (transaction.BusinessPartner?.CustomerCode != null)
                {
                    transaction.BusinessPartner.OtherBuyers = await _context.BusinessPartners
                        .Where(bp => bp.CustomerCode == transaction.BusinessPartner.CustomerCode)
                        .ToListAsync();
                }
            }

            ViewBag.SalesTransactions = result;
            return View();
        }
    }
}

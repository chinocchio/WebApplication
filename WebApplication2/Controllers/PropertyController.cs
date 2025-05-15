using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class PropertyController : Controller
    {
        private readonly DataContext _context;

        public PropertyController(DataContext context)
        {
            _context = context;
        }

        //GET: Property/Index
        public IActionResult Index()
        {
            return View();
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

        // GET: Property/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Property property)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                // Log errors here to see what validation failed
                Console.WriteLine("ModelState Invalid:");
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                return View(property);
            }

            Console.WriteLine("Property Received:");
            Console.WriteLine($"Type: {property.PropertyType}, Project: {property.ProjectName}, Phase: {property.BuildingPhase}, Unit: {property.UnitCode}");

            _context.Add(property);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Property/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .FirstOrDefaultAsync(m => m.PropertyId == id);
            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        // GET: Property/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            return View(property);
        }

        // POST: Property/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Property property)
        {
            if (id != property.PropertyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(property.PropertyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(property);
        }

        // GET: Property/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .FirstOrDefaultAsync(m => m.PropertyId == id);
            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        // POST: Property/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                _context.Properties.Remove(property);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.PropertyId == id);
        }
    }
}

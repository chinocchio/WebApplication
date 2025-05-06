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
        // Displays the list of data of a property if searched.
        public async Task<IActionResult> Index(string? searchTerm)
        {
            /*
             * Pinalitan ko from Properties to SalesTransactions bale sa SalesTransaction table tayo nagselect
             * Eto yung query: Select ka sa SalesTransaction table tapos Include yung mga related tables
             */
            var query = _context.SalesTransactions
                        .AsNoTracking()
                        .Include(st => st.Properties)
                        .Include(st => st.BusinessPartner)
                        .Include(st => st.SalesProponent)
                        .Include(st => st.PaymentTerm)
                        .Include(st => st.CreditReview)
                        .Include(st => st.BuyerDocument)
                        .Include(st => st.SalesDocument)
                        .AsQueryable();

            // Eto yung search functionality, check pag may laman yung searchTerm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // Convert to lowercase for case-insensitive search

                /*
                 * Yung "st" galing yan sa query sa taas base sa mga yan dun tayo magsearch bale yung records galing sa 
                 * SalesTransaction table dun hahanapin yung mga i-eenter the search bar
                 */
                query = query.Where(st =>
                    st.ContractNumber.ToString().Contains(searchTerm) ||
                    st.BusinessPartner.Fullname.ToLower().Contains(searchTerm) ||
                    st.BusinessPartner.CustomerCode.ToString().Contains(searchTerm) ||
                    st.Properties.UnitCode.ToLower().Contains(searchTerm)
                );
            }

            /*
             * Para mas madali makita yung mga recent na transaction, nag order by tayo
             * sa HoldingDate in descending order
             */
            query = query.OrderByDescending(st => st.HoldingDate);

            var salesTransactions = await query.ToListAsync();

            var submittedDocuments = await _context.SubmittedDocuments
                .AsNoTracking()
                .ToListAsync();

            var allDocumentsForSubmission = await _context.DocumentForSubmissions
                .AsNoTracking()
                .ToListAsync();

            var result = salesTransactions.Select(st => {
                // Find all submitted for this contract
                var submittedForThis = submittedDocuments
                    .Where(doc => doc.ContractNumber == st.ContractNumber)
                    .ToList();

                // Extract their DocumentCodes
                var submittedDocCodes = submittedForThis.Select(doc => doc.DocumentCode).ToHashSet();

                // Filter out the submitted ones from required list based on DocumentCode
                var remainingRequired = allDocumentsForSubmission
                    .Where(doc =>!submittedDocCodes.Contains(doc.DocumentCode))
                    .ToList();

                return new SalesTransactionWithDocumentsViewModel
                {
                    SalesTransaction = st,
                    SubmittedDocuments = submittedForThis,
                    DocumentsForSubmission = remainingRequired
                };
            }).ToList();


            // Load OtherBuyers for each transaction
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
            // For each transaction, load all buyers with the same customer code
            foreach (var transaction in salesTransactions)
            {
                if (transaction.BusinessPartner?.CustomerCode != null)
                {
                    transaction.BusinessPartner.OtherBuyers = await _context.BusinessPartners
                        .Where(bp => bp.CustomerCode == transaction.BusinessPartner.CustomerCode)
                        .ToListAsync();
                }
            }

            var model = new PropertyListViewModel
            {
                SalesTransactions = result,
                SearchTerm = searchTerm
            };


            //var model = new PropertyListViewModel
            //{
            //    SalesTransactions = salesTransactions,
            //    SearchTerm = searchTerm
            //};


            /*
             * Tapos pasa nating sa PropertyListViewModel yung mga nakuha nating data
             * na gagamitin natin sa view
             */
            //var model = new PropertyListViewModel
            //{
            //    SalesTransactions = await query.ToListAsync(),
            //    SearchTerm = searchTerm
            //};

            // Tapos papasa natin sa ModelView yung result nung controller na to
            return View(model);
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

    }
}

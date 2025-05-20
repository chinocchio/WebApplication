using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using ExcelDataReader;
using System.Data;
using System.Text;

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
                    (st.BusinessPartner != null && st.BusinessPartner.Fullname != null && st.BusinessPartner.Fullname.ToLower().Contains(searchTerm)) ||
                    (st.BusinessPartner != null && st.BusinessPartner.CustomerCode != null && st.BusinessPartner.CustomerCode.Contains(searchTerm)) ||
                    (st.Properties != null && st.Properties.UnitCode != null && st.Properties.UnitCode.ToLower().Contains(searchTerm)) ||
                    (st.ProponentBpNumber != null && st.ProponentBpNumber.ToString().Contains(searchTerm))
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
                return RedirectToAction(nameof(SearchResults));
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
            if (property == null)
            {
                return Json(new { success = false, message = "Property not found." });
            }

            try
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Property deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting property: " + ex.Message });
            }
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.PropertyId == id);
        }

        // GET: Property/Import
        public IActionResult Import()
        {
            return View(new PropertyImportViewModel());
        }

        // POST: Property/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(PropertyImportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ImportErrors = new List<string>();
            model.SuccessCount = 0;
            model.ErrorCount = 0;

            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using (var stream = model.File.OpenReadStream())
                {
                    IExcelDataReader reader;
                    
                    if (Path.GetExtension(model.File.FileName).ToLowerInvariant() == ".csv")
                    {
                        reader = ExcelReaderFactory.CreateCsvReader(stream);
                    }
                    else
                    {
                        reader = ExcelReaderFactory.CreateReader(stream);
                    }

                    using (reader)
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });

                        DataTable dataTable = result.Tables[0];

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            try
                            {
                                var row = dataTable.Rows[i];

                                // Get and validate contract number
                                var contractNumberStr = row["ContractNumber"]?.ToString();
                                if (string.IsNullOrEmpty(contractNumberStr))
                                {
                                    model.ImportErrors.Add($"Row {i + 2}: Contract Number is required");
                                    model.ErrorCount++;
                                    continue;
                                }

                                if (!long.TryParse(contractNumberStr, out long contractNumber))
                                {
                                    model.ImportErrors.Add($"Row {i + 2}: Invalid Contract Number format");
                                    model.ErrorCount++;
                                    continue;
                                }

                                // Check if sales transaction exists
                                var salesTransaction = await _context.SalesTransactions
                                    .FirstOrDefaultAsync(st => st.ContractNumber == contractNumber);

                                if (salesTransaction == null)
                                {
                                    model.ImportErrors.Add($"Row {i + 2}: No matching Sales Transaction found for Contract Number {contractNumber}");
                                    model.ErrorCount++;
                                    continue;
                                }

                                // Get and validate property fields
                                var unitCode = row["UnitCode"]?.ToString();
                                var projectName = row["ProjectName"]?.ToString();
                                var buildingPhase = row["BuildingPhase"]?.ToString();
                                var propertyType = row["PropertyType"]?.ToString();

                                // Validate required fields
                                if (string.IsNullOrEmpty(unitCode))
                                {
                                    model.ImportErrors.Add($"Row {i + 2}: Unit Code is required");
                                    model.ErrorCount++;
                                    continue;
                                }
                                if (string.IsNullOrEmpty(projectName))
                                {
                                    model.ImportErrors.Add($"Row {i + 2}: Project Name is required");
                                    model.ErrorCount++;
                                    continue;
                                }
                                if (string.IsNullOrEmpty(buildingPhase))
                                {
                                    model.ImportErrors.Add($"Row {i + 2}: Building Phase is required");
                                    model.ErrorCount++;
                                    continue;
                                }
                                if (string.IsNullOrEmpty(propertyType))
                                {
                                    model.ImportErrors.Add($"Row {i + 2}: Property Type is required");
                                    model.ErrorCount++;
                                    continue;
                                }

                                // Check if property already exists
                                var existingProperty = await _context.Properties
                                    .FirstOrDefaultAsync(p => p.UnitCode == unitCode);

                                if (existingProperty != null)
                                {
                                    // If property exists, update the sales transaction to link to it
                                    salesTransaction.PropertyId = existingProperty.PropertyId;
                                    _context.Update(salesTransaction);
                                    await _context.SaveChangesAsync();
                                    model.SuccessCount++;
                                    continue;
                                }

                                // Create new property
                                var property = new Property
                                {
                                    PropertyType = propertyType,
                                    ProjectName = projectName,
                                    BuildingPhase = buildingPhase,
                                    UnitCode = unitCode
                                };

                                _context.Properties.Add(property);
                                await _context.SaveChangesAsync();

                                // Update sales transaction with the new property ID
                                salesTransaction.PropertyId = property.PropertyId;
                                _context.Update(salesTransaction);
                                await _context.SaveChangesAsync();

                                model.SuccessCount++;
                            }
                            catch (Exception ex)
                            {
                                model.ImportErrors.Add($"Row {i + 2}: {ex.Message}");
                                model.ErrorCount++;
                            }
                        }
                    }
                }

                if (model.ErrorCount == 0 && model.SuccessCount > 0)
                {
                    TempData["SuccessMessage"] = $"Successfully imported {model.SuccessCount} properties and linked them to sales transactions.";
                    return RedirectToAction(nameof(Index));
                }
                else if (model.SuccessCount == 0)
                {
                    ModelState.AddModelError("", "No records were imported. Please check your file format and data.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error processing file: {ex.Message}");
            }

            return View(model);
        }
    }
}

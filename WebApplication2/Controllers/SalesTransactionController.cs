using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using ExcelDataReader;
using System.Data;
using System.Text;
using OfficeOpenXml;

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

        // GET: SalesTransaction
        // Para sa listahan ng mga benta, boss. Dito mo makikita lahat ng transaksyon!
        public IActionResult Index()
        {
            return RedirectToAction("SearchResults", "Property");
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

        // GET: SalesTransaction/Edit/5
        // Para sa pag-edit ng existing na benta, boss. Ingat lang sa pagbabago!
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesTransaction = _context.SalesTransactions
                .Include(st => st.BusinessPartner)
                .FirstOrDefault(st => st.SalesTransactionId == id);

            if (salesTransaction == null)
            {
                return NotFound();
            }

            var model = new SalesTransactionCreateViewModel
            {
                SalesTransactionId = salesTransaction.SalesTransactionId,
                ContractNumber = salesTransaction.ContractNumber,
                TypeOfSale = salesTransaction.TypeOfSale,
                TransactionType = salesTransaction.TransactionType,
                PromoDiscount = salesTransaction.PromoDiscount,
                StatusInGeneral = salesTransaction.StatusInGeneral,
                Milestone = salesTransaction.Milestone,
                NewColorStatus = salesTransaction.NewColorStatus,
                SelectedPropertyId = salesTransaction.PropertyId,
                SelectedBusinessPartnerId = salesTransaction.BusinessPartnerId,
                SelectedProponentBpNumber = salesTransaction.ProponentBpNumber,
                Role = salesTransaction.BusinessPartner.Role,
                Fullname = salesTransaction.BusinessPartner.Fullname,
                CustomerCode = salesTransaction.BusinessPartner.CustomerCode,
                ClientBase = salesTransaction.BusinessPartner.ClientBase,
                IdSubmitted = salesTransaction.BusinessPartner.IdSubmitted,
                IdDateSubmitted = salesTransaction.BusinessPartner.IdDateSubmitted,
                EmailAddress = salesTransaction.BusinessPartner.EmailAddress,
                ContactNumber = salesTransaction.BusinessPartner.ContactNumber,

                Properties = _context.Properties
                    .Select(p => new SelectListItem
                    {
                        Value = p.PropertyId.ToString(),
                        Text = $"{p.ProjectName} - {p.BuildingPhase} - {p.UnitCode}"
                    }).ToList(),

                ExistingBusinessPartners = _context.BusinessPartners
                    .Select(bp => new SelectListItem
                    {
                        Value = bp.BusinessPartnerId.ToString(),
                        Text = $"{bp.Role} - {bp.Fullname} - {bp.CustomerCode}"
                    }).ToList(),

                SalesProponents = _context.SalesProponents
                    .Select(sp => new SelectListItem
                    {
                        Value = sp.ProponentBpNumber.ToString(),
                        Text = $"{sp.Roles} - {sp.Fullname} - {sp.ProponentBpNumber}"
                    }).ToList()
            };

            return View(model);
        }

        // POST: SalesTransaction/Edit/5
        // Para sa pag-save ng mga pagbabago sa benta, boss. Double check mo muna!
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SalesTransactionCreateViewModel model)
        {
            if (id != model.SalesTransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var salesTransaction = _context.SalesTransactions
                        .Include(st => st.BusinessPartner)
                        .FirstOrDefault(st => st.SalesTransactionId == id);

                    if (salesTransaction == null)
                    {
                        return NotFound();
                    }

                    // Update Business Partner
                    salesTransaction.BusinessPartner.Role = model.Role;
                    salesTransaction.BusinessPartner.Fullname = model.Fullname;
                    salesTransaction.BusinessPartner.CustomerCode = model.CustomerCode;
                    salesTransaction.BusinessPartner.ClientBase = model.ClientBase;
                    salesTransaction.BusinessPartner.IdSubmitted = model.IdSubmitted;
                    salesTransaction.BusinessPartner.IdDateSubmitted = model.IdDateSubmitted;
                    salesTransaction.BusinessPartner.EmailAddress = model.EmailAddress;
                    salesTransaction.BusinessPartner.ContactNumber = model.ContactNumber;

                    // Update Sales Transaction
                    salesTransaction.ContractNumber = model.ContractNumber;
                    salesTransaction.TypeOfSale = model.TypeOfSale;
                    salesTransaction.TransactionType = model.TransactionType;
                    salesTransaction.PromoDiscount = model.PromoDiscount;
                    salesTransaction.StatusInGeneral = model.StatusInGeneral;
                    salesTransaction.Milestone = model.Milestone;
                    salesTransaction.NewColorStatus = model.NewColorStatus;
                    salesTransaction.PropertyId = model.SelectedPropertyId.Value;
                    salesTransaction.ProponentBpNumber = model.SelectedProponentBpNumber;

                    _context.Update(salesTransaction);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesTransactionExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            model.Properties = _context.Properties
                .Select(p => new SelectListItem
                {
                    Value = p.PropertyId.ToString(),
                    Text = $"{p.ProjectName} - {p.BuildingPhase} - {p.UnitCode}"
                }).ToList();

            model.ExistingBusinessPartners = _context.BusinessPartners
                .Select(bp => new SelectListItem
                {
                    Value = bp.BusinessPartnerId.ToString(),
                    Text = $"{bp.Role} - {bp.Fullname} - {bp.CustomerCode}"
                }).ToList();

            model.SalesProponents = _context.SalesProponents
                .Select(sp => new SelectListItem
                {
                    Value = sp.ProponentBpNumber.ToString(),
                    Text = $"{sp.Roles} - {sp.Fullname} - {sp.ProponentBpNumber}"
                }).ToList();

            return View(model);
        }

        // POST: SalesTransaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesTransaction = await _context.SalesTransactions
                .Include(st => st.BusinessPartner)
                .Include(st => st.PaymentTerm)
                .Include(st => st.CreditReview)
                .Include(st => st.SalesDocument)
                .FirstOrDefaultAsync(st => st.SalesTransactionId == id);

            if (salesTransaction == null)
            {
                return Json(new { success = false, message = "Sales transaction not found." });
            }

            try
            {
                // Remove related records first
                if (salesTransaction.PaymentTerm != null)
                    _context.PaymentTerms.Remove(salesTransaction.PaymentTerm);
                if (salesTransaction.CreditReview != null)
                    _context.CreditReviews.Remove(salesTransaction.CreditReview);
                if (salesTransaction.SalesDocument != null)
                    _context.SalesDocuments.Remove(salesTransaction.SalesDocument);

                // Remove the sales transaction
                _context.SalesTransactions.Remove(salesTransaction);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Sales transaction deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting sales transaction: " + ex.Message });
            }
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

            if (!string.IsNullOrWhiteSpace(searchTerm))
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
            // If searchTerm is null or empty, query returns all records

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
                    .Where(doc => doc.ContractNumber.HasValue && doc.ContractNumber.Value == st.ContractNumber)
                    .ToList();

                var submittedDocCodes = submittedForThis.Select(doc => doc.DocumentCode).ToHashSet();

                var remainingRequired = allDocumentsForSubmission
                    .Where(doc =>!submittedDocCodes.Contains(doc.DocumentCode))
                    .ToList();

                var matchingLedgers = _context.BuyerLedgers
                   .AsNoTracking()
                   .Where(bl => bl.ContractNumber.HasValue && bl.ContractNumber.Value == st.ContractNumber)
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

        // GET: SalesTransaction/Import
        public IActionResult Import()
        {
            return View(new SalesTransactionImportViewModel());
        }

        // POST: SalesTransaction/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(SalesTransactionImportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ImportErrors = new List<string>();
            model.SuccessCount = 0;
            model.ErrorCount = 0;

            if (model.File == null || model.File.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file to import");
                return View(model);
            }

            try
            {
                // Register encoding provider for Excel reading
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using (var stream = model.File.OpenReadStream())
                {
                    IExcelDataReader reader;
                    
                    // Check if it's a CSV file
                    if (Path.GetExtension(model.File.FileName).ToLowerInvariant() == ".csv")
                    {
                        reader = ExcelReaderFactory.CreateCsvReader(stream);
                        System.Diagnostics.Debug.WriteLine("Processing CSV file");
                    }
                    else
                    {
                        reader = ExcelReaderFactory.CreateReader(stream);
                        System.Diagnostics.Debug.WriteLine("Processing Excel file");
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

                        // Process the first sheet
                        DataTable dataTable = result.Tables[0];
                        var transactions = new List<SalesTransaction>();

                        // Debug: Print column names
                        System.Diagnostics.Debug.WriteLine("Columns found:");
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            System.Diagnostics.Debug.WriteLine($"Column: {column.ColumnName}");
                        }

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            try
                            {
                                var row = dataTable.Rows[i];
                                
                                // Debug: Print row values
                                System.Diagnostics.Debug.WriteLine($"\nProcessing Row {i + 2}:");
                                foreach (DataColumn column in dataTable.Columns)
                                {
                                    System.Diagnostics.Debug.WriteLine($"{column.ColumnName}: {row[column]}");
                                }

                                // Parse contract number
                                var contractNumberStr = row["ContractNumber"]?.ToString();
                                System.Diagnostics.Debug.WriteLine($"Parsing Contract Number: {contractNumberStr}");
                                if (!long.TryParse(contractNumberStr, out long contractNumber))
                                {
                                    model.ImportErrors.Add($"Row {i + 2}: Invalid Contract Number: {contractNumberStr}");
                                    model.ErrorCount++;
                                    continue;
                                }

                                // Parse holding date with specific format M/d/yy
                                var dateString = row["HoldingDate"]?.ToString();
                                System.Diagnostics.Debug.WriteLine($"Parsing Date: {dateString}");
                                DateOnly holdingDate;
                                if (DateTime.TryParseExact(dateString, 
                                    new[] { "M/d/yy", "MM/dd/yy", "M/d/yyyy", "MM/dd/yyyy" }, 
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None, 
                                    out DateTime parsedDate))
                                {
                                    holdingDate = DateOnly.FromDateTime(parsedDate);
                                }
                                else
                                {
                                    model.ImportErrors.Add($"Row {i + 2}: Invalid Holding Date format. Use format like 5/21/22. Got: {dateString}");
                                    model.ErrorCount++;
                                    continue;
                                }

                                var transaction = new SalesTransaction
                                {
                                    ContractNumber = contractNumber,
                                    TypeOfSale = row["TypeOfSale"]?.ToString(),
                                    HoldingDate = holdingDate,
                                    TransactionType = row["TransactionType"]?.ToString(),
                                    StatusInGeneral = row["StatusInGeneral"]?.ToString(),
                                    Milestone = row["Milestone"]?.ToString(),
                                    NewColorStatus = row["NewColorStatus"]?.ToString()
                                };

                                transactions.Add(transaction);
                                model.SuccessCount++;
                                System.Diagnostics.Debug.WriteLine($"Successfully created transaction for Contract Number: {contractNumber}");
                            }
                            catch (Exception ex)
                            {
                                model.ImportErrors.Add($"Row {i + 2}: {ex.Message}");
                                model.ErrorCount++;
                                System.Diagnostics.Debug.WriteLine($"Error processing row {i + 2}: {ex.Message}");
                            }
                        }

                        System.Diagnostics.Debug.WriteLine($"Total transactions to save: {transactions.Count}");
                        if (transactions.Any())
                        {
                            await _context.SalesTransactions.AddRangeAsync(transactions);
                            await _context.SaveChangesAsync();
                            System.Diagnostics.Debug.WriteLine("Transactions saved to database");
                        }
                    }
                }

                if (model.ErrorCount == 0 && model.SuccessCount > 0)
                {
                    TempData["SuccessMessage"] = $"Successfully imported {model.SuccessCount} transactions.";
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
                return View(model);
            }

            return View(model);
        }

        // GET: SalesTransaction/DownloadTemplate
        public IActionResult DownloadTemplate()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Chino");
            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sales Transaction Import Template");

                // Add headers
                worksheet.Cells[1, 1].Value = "ContractNumber";
                worksheet.Cells[1, 2].Value = "TypeOfSale";
                worksheet.Cells[1, 3].Value = "HoldingDate";
                worksheet.Cells[1, 4].Value = "TransactionType";
                worksheet.Cells[1, 5].Value = "StatusInGeneral";
                worksheet.Cells[1, 6].Value = "Milestone";
                worksheet.Cells[1, 7].Value = "NewColorStatus";

                // Add sample data
                worksheet.Cells[2, 1].Value = "12345";
                worksheet.Cells[2, 2].Value = "New Sale";
                worksheet.Cells[2, 3].Value = "5/21/2024";
                worksheet.Cells[2, 4].Value = "Regular";
                worksheet.Cells[2, 5].Value = "Active";
                worksheet.Cells[2, 6].Value = "Initial";
                worksheet.Cells[2, 7].Value = "GREEN";

                // Style the header row
                using (var range = worksheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                // Convert to byte array
                var content = package.GetAsByteArray();

                // Return the file
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalesTransactionImportTemplate.xlsx");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBuyersLedger(BuyerLedger model)
        {
            if (ModelState.IsValid)
            {
                var salesTransaction = await _context.SalesTransactions
                    .Include(s => s.Properties)
                    .Include(s => s.BusinessPartner)
                    .FirstOrDefaultAsync(s => s.ContractNumber == model.ContractNumber);

                if (salesTransaction == null)
                {
                    // Return JSON error instead of redirect
                    return Json(new { success = false, message = "Invalid contract number." });
                }

                model.UnitCode = salesTransaction.Properties?.UnitCode;
                model.CustomerCode = salesTransaction.BusinessPartner?.CustomerCode;
                model.WhenDue = DateOnly.FromDateTime(DateTime.Now);
                model.PaymentTermSchedule = DateOnly.FromDateTime(DateTime.Now);

                _context.BuyerLedgers.Add(model);
                await _context.SaveChangesAsync();

                // Return JSON success
                return Json(new { success = true, message = "Buyer's ledger entry created successfully." });
            }

            // Return JSON validation error
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = "Validation failed. Please check your input.", errors });
        }
    }
}

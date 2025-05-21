using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using ExcelDataReader;
using System.Data;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace WebApplication2.Controllers
{
    public class CreditReviewController : Controller
    {
        private readonly DataContext _context;

        public CreditReviewController(DataContext context)
        {
            _context = context;
        }

        // GET: CreditReview/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: CreditReview/Import
        public IActionResult Import()
        {
            return View(new CreditReviewImportViewModel());
        }

        // POST: CreditReview/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(CreditReviewImportViewModel model)
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

                                // Create new credit review
                                var creditReview = new CreditReview
                                {
                                    CreditReviewResult = row["CreditReviewResult"]?.ToString() ?? null,
                                    CMAPResult = row["CMAPResult"]?.ToString() ?? null,
                                    CreditReviewRemarks = row["CreditReviewRemarks"]?.ToString() ?? null,
                                    NDIStatus = row["NDIStatus"]?.ToString() ?? null,
                                    IsBankApporvable = row["IsBankApporvable"]?.ToString() ?? null,
                                    RedTag = row["RedTag"]?.ToString() ?? null,
                                    RedTagReason = row["RedTagReason"]?.ToString() ?? null,
                                    CiCompletionDate = string.IsNullOrWhiteSpace(row["CiCompletionDate"]?.ToString()) ? null : DateOnly.Parse(row["CiCompletionDate"].ToString()),
                                    ReasonForPurchase = row["ReasonForPurchase"]?.ToString() ?? null,
                                    FirstHomeInPH = row["FirstHomeInPH"]?.ToString() ?? null,
                                    NumberOfHomesInPH = int.TryParse(row["NumberOfHomesInPH"]?.ToString() ?? "", out int homes) ? homes : null,
                                    WithOtherCPGIUnits = row["WithOtherCPGIUnits"]?.ToString() ?? null,
                                    ProjectOrUnitCodeOfOtherCPGIUnit = row["ProjectOrUnitCodeOfOtherCPGIUnit"]?.ToString() ?? null,
                                    IncomeDeclaredPb = double.TryParse(row["IncomeDeclaredPb"]?.ToString() ?? "", out double incomePb) ? incomePb : null,
                                    IncomeDeclaredCb = double.TryParse(row["IncomeDeclaredCb"]?.ToString() ?? "", out double incomeCb) ? incomeCb : null,
                                    TotalIncomeCombined = double.TryParse(row["TotalIncomeCombined"]?.ToString() ?? "", out double totalIncome) ? totalIncome : null,
                                    TypeOfIncome = row["TypeOfIncome"]?.ToString() ?? null,
                                    NdiRate = row["NdiRate"]?.ToString() ?? null,
                                    NetDisposableIncome = double.TryParse(row["NetDisposableIncome"]?.ToString() ?? "", out double ndi) ? ndi : null,
                                    OtherLoans = double.TryParse(row["OtherLoans"]?.ToString() ?? "", out double loans) ? loans : null,
                                    NetNdi = double.TryParse(row["NetNdi"]?.ToString() ?? "", out double netNdi) ? netNdi : null,
                                    NdiVsBankMaTobAmt = double.TryParse(row["NdiVsBankMaTobAmt"]?.ToString() ?? "", out double ndiVsBank) ? ndiVsBank : null,
                                    PercentOfNdiVsMa = row["PercentOfNdiVsMa"]?.ToString() ?? null,
                                    NdiCategory = row["NdiCategory"]?.ToString() ?? null,
                                    MaxTerm = int.TryParse(row["MaxTerm"]?.ToString() ?? "0", out int maxTerm) ? maxTerm : 0,
                                    EstimatedMaxTerm = row["EstimatedMaxTerm"]?.ToString() ?? null,
                                    PersonaCategory = row["PersonaCategory"]?.ToString() ?? null,
                                    ImmigrantOrNonImmigrant = row["ImmigrantOrNonImmigrant"]?.ToString() ?? null,
                                    HighRishk = row["HighRishk"]?.ToString() ?? null,
                                    HighRiskFactors = row["HighRiskFactors"]?.ToString() ?? null
                                };

                                _context.CreditReviews.Add(creditReview);
                                await _context.SaveChangesAsync();

                                // Update sales transaction with the new credit review ID
                                salesTransaction.CreditReviewId = creditReview.CreditReviewId;
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
                    TempData["SuccessMessage"] = $"Successfully imported {model.SuccessCount} credit reviews and linked them to sales transactions.";
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

        // GET: CreditReview/DownloadTemplate
        public IActionResult DownloadTemplate()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Chino");
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Credit Review Import Template");

                // Add headers
                var headers = new[]
                {
                    "ContractNumber",
                    "CreditReviewResult",
                    "CMAPResult",
                    "CreditReviewRemarks",
                    "NDIStatus",
                    "IsBankApporvable",
                    "RedTag",
                    "RedTagReason",
                    "CiCompletionDate",
                    "ReasonForPurchase",
                    "FirstHomeInPH",
                    "NumberOfHomesInPH",
                    "WithOtherCPGIUnits",
                    "ProjectOrUnitCodeOfOtherCPGIUnit",
                    "IncomeDeclaredPb",
                    "IncomeDeclaredCb",
                    "TotalIncomeCombined",
                    "TypeOfIncome",
                    "NdiRate",
                    "NetDisposableIncome",
                    "OtherLoans",
                    "NetNdi",
                    "NdiVsBankMaTobAmt",
                    "PercentOfNdiVsMa",
                    "NdiCategory",
                    "MaxTerm",
                    "EstimatedMaxTerm",
                    "PersonaCategory",
                    "ImmigrantOrNonImmigrant",
                    "HighRishk",
                    "HighRiskFactors"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }

                // Add sample data
                worksheet.Cells[2, 1].Value = "12345";
                worksheet.Cells[2, 2].Value = "Approved";
                worksheet.Cells[2, 3].Value = "Pass";
                worksheet.Cells[2, 4].Value = "Good credit history";
                worksheet.Cells[2, 5].Value = "Active";
                worksheet.Cells[2, 6].Value = "Yes";
                worksheet.Cells[2, 7].Value = "No";
                worksheet.Cells[2, 8].Value = "";
                worksheet.Cells[2, 9].Value = "2024-03-20";
                worksheet.Cells[2, 10].Value = "Primary Residence";
                worksheet.Cells[2, 11].Value = "Yes";
                worksheet.Cells[2, 12].Value = "1";
                worksheet.Cells[2, 13].Value = "No";
                worksheet.Cells[2, 14].Value = "";
                worksheet.Cells[2, 15].Value = "50000";
                worksheet.Cells[2, 16].Value = "30000";
                worksheet.Cells[2, 17].Value = "80000";
                worksheet.Cells[2, 18].Value = "Employment";
                worksheet.Cells[2, 19].Value = "40";
                worksheet.Cells[2, 20].Value = "32000";
                worksheet.Cells[2, 21].Value = "5000";
                worksheet.Cells[2, 22].Value = "27000";
                worksheet.Cells[2, 23].Value = "25000";
                worksheet.Cells[2, 24].Value = "92.6";
                worksheet.Cells[2, 25].Value = "A";
                worksheet.Cells[2, 26].Value = "30";
                worksheet.Cells[2, 27].Value = "25";
                worksheet.Cells[2, 28].Value = "Regular";
                worksheet.Cells[2, 29].Value = "Non-Immigrant";
                worksheet.Cells[2, 30].Value = "No";
                worksheet.Cells[2, 31].Value = "";

                // Style the header row
                using (var range = worksheet.Cells[1, 1, 1, headers.Length])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                // Convert to byte array
                var content = package.GetAsByteArray();

                // Return the file
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CreditReviewImportTemplate.xlsx");
            }
        }
    }
} 
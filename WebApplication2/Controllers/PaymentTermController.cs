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
    public class PaymentTermController : Controller
    {
        private readonly DataContext _context;

        public PaymentTermController(DataContext context)
        {
            _context = context;
        }

        // GET: PaymentTerm/Import
        public IActionResult Import()
        {
            return View(new PaymentTermImportViewModel());
        }

        // POST: PaymentTerm/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(PaymentTermImportViewModel model)
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

                                // Create new payment term
                                var paymentTerm = new PaymentTerm
                                {
                                    RfDatePaid = ParseDateOnly(row["RfDatePaid"]?.ToString()),
                                    RfAmountPaidToUnit = ParseDouble(row["RfAmountPaidToUnit"]?.ToString()),
                                    RfAmountPaidToGMTOE = ParseDouble(row["RfAmountPaidToGMTOE"]?.ToString()),
                                    RfAmountGMTOE_Unit = ParseDouble(row["RfAmountGMTOE_Unit"]?.ToString()),
                                    RfDateCredited = ParseDateOnly(row["RfDateCredited"]?.ToString()),
                                    RfOrNumber = row["RfOrNumber"]?.ToString(),
                                    Paymentterm = row["Paymentterm"]?.ToString(),
                                    PercentTOB = row["PercentTOB"]?.ToString(),
                                    TOBModeOfPayment = row["TOBModeOfPayment"]?.ToString(),
                                    TOB = ParseDouble(row["TOB"]?.ToString()),
                                    EstimatedBankMAFor7Point5Percent = ParseDouble(row["EstimatedBankMAFor7Point5Percent"]?.ToString()),
                                    StartDate1stMA = ParseDateOnly(row["StartDate1stMA"]?.ToString()),
                                    UnitParking = ParseDouble(row["UnitParking"]?.ToString()),
                                    TFee = ParseDouble(row["TFee"]?.ToString()),
                                    AmountPaidToUnit = ParseDouble(row["AmountPaidToUnit"]?.ToString()),
                                    AmountPaidToTF = ParseDouble(row["AmountPaidToTF"]?.ToString()),
                                    DatePaid = ParseDateOnly(row["DatePaid"]?.ToString()),
                                    FirstMAOrNumber = row["FirstMAOrNumber"]?.ToString(),
                                    PaymentDueDate = ParseInt(row["PaymentDueDate"]?.ToString()),
                                    Unit = ParseDouble(row["Unit"]?.ToString()),
                                    TransferFee = ParseDouble(row["TransferFee"]?.ToString()),
                                    Total = ParseDouble(row["Total"]?.ToString()),
                                    PercentPayment = row["PercentPayment"]?.ToString(),
                                    PaymentCategory = row["PaymentCategory"]?.ToString()
                                };

                                // Check if payment term already exists for this contract
                                var existingPaymentTerm = await _context.PaymentTerms
                                    .FirstOrDefaultAsync(pt => pt.SalesTransaction.Any(st => st.ContractNumber == contractNumber));

                                if (existingPaymentTerm != null)
                                {
                                    // Update existing payment term
                                    _context.Entry(existingPaymentTerm).CurrentValues.SetValues(paymentTerm);
                                    await _context.SaveChangesAsync();
                                    model.SuccessCount++;
                                    continue;
                                }

                                // Add new payment term
                                _context.PaymentTerms.Add(paymentTerm);
                                await _context.SaveChangesAsync();

                                // Link payment term to sales transaction
                                salesTransaction.PaymentTermId = paymentTerm.PaymentTermId;
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
                    TempData["SuccessMessage"] = $"Successfully imported {model.SuccessCount} payment terms.";
                    return RedirectToAction(nameof(Import));
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

        // GET: PaymentTerm/DownloadTemplate
        public IActionResult DownloadTemplate()
        {
            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Payment Term Import Template");

                // Add headers
                worksheet.Cells[1, 1].Value = "ContractNumber";
                worksheet.Cells[1, 2].Value = "RfDatePaid";
                worksheet.Cells[1, 3].Value = "RfAmountPaidToUnit";
                worksheet.Cells[1, 4].Value = "RfAmountPaidToGMTOE";
                worksheet.Cells[1, 5].Value = "RfAmountGMTOE_Unit";
                worksheet.Cells[1, 6].Value = "RfDateCredited";
                worksheet.Cells[1, 7].Value = "RfOrNumber";
                worksheet.Cells[1, 8].Value = "Paymentterm";
                worksheet.Cells[1, 9].Value = "PercentTOB";
                worksheet.Cells[1, 10].Value = "TOBModeOfPayment";
                worksheet.Cells[1, 11].Value = "TOB";
                worksheet.Cells[1, 12].Value = "EstimatedBankMAFor7Point5Percent";
                worksheet.Cells[1, 13].Value = "StartDate1stMA";
                worksheet.Cells[1, 14].Value = "UnitParking";
                worksheet.Cells[1, 15].Value = "TFee";
                worksheet.Cells[1, 16].Value = "AmountPaidToUnit";
                worksheet.Cells[1, 17].Value = "AmountPaidToTF";
                worksheet.Cells[1, 18].Value = "DatePaid";
                worksheet.Cells[1, 19].Value = "FirstMAOrNumber";
                worksheet.Cells[1, 20].Value = "PaymentDueDate";
                worksheet.Cells[1, 21].Value = "Unit";
                worksheet.Cells[1, 22].Value = "TransferFee";
                worksheet.Cells[1, 23].Value = "Total";
                worksheet.Cells[1, 24].Value = "PercentPayment";
                worksheet.Cells[1, 25].Value = "PaymentCategory";

                // Add sample data
                worksheet.Cells[2, 1].Value = "12345";
                worksheet.Cells[2, 2].Value = "2024-03-20";
                worksheet.Cells[2, 3].Value = "1000.00";
                worksheet.Cells[2, 4].Value = "500.00";
                worksheet.Cells[2, 5].Value = "1500.00";
                worksheet.Cells[2, 6].Value = "2024-03-21";
                worksheet.Cells[2, 7].Value = "OR123456";
                worksheet.Cells[2, 8].Value = "Monthly";
                worksheet.Cells[2, 9].Value = "20%";
                worksheet.Cells[2, 10].Value = "Bank Transfer";
                worksheet.Cells[2, 11].Value = "50000.00";
                worksheet.Cells[2, 12].Value = "3750.00";
                worksheet.Cells[2, 13].Value = "2024-04-01";
                worksheet.Cells[2, 14].Value = "250000.00";
                worksheet.Cells[2, 15].Value = "5000.00";
                worksheet.Cells[2, 16].Value = "245000.00";
                worksheet.Cells[2, 17].Value = "5000.00";
                worksheet.Cells[2, 18].Value = "2024-03-20";
                worksheet.Cells[2, 19].Value = "MA123456";
                worksheet.Cells[2, 20].Value = "1";
                worksheet.Cells[2, 21].Value = "250000.00";
                worksheet.Cells[2, 22].Value = "5000.00";
                worksheet.Cells[2, 23].Value = "255000.00";
                worksheet.Cells[2, 24].Value = "100%";
                worksheet.Cells[2, 25].Value = "Full Payment";

                // Style the header row
                using (var range = worksheet.Cells[1, 1, 1, 25])
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
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PaymentTermImportTemplate.xlsx");
            }
        }

        private DateOnly? ParseDateOnly(string? value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return DateOnly.TryParse(value, out DateOnly result) ? result : null;
        }

        private double? ParseDouble(string? value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return double.TryParse(value, out double result) ? result : null;
        }

        private int? ParseInt(string? value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return int.TryParse(value, out int result) ? result : null;
        }
    }
}
using Microsoft.AspNetCore.Mvc;
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
    public class BusinessPartnerController : Controller
    {
        private readonly DataContext _context;

        public BusinessPartnerController(DataContext context)
        {
            _context = context;
        }

        // GET: BusinessPartner/Import
        public IActionResult Import()
        {
            return View(new BusinessPartnerImportViewModel());
        }

        // POST: BusinessPartner/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(BusinessPartnerImportViewModel model)
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
                        var businessPartners = new List<BusinessPartner>();
                        var contractNumbers = new List<long>();

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            try
                            {
                                var row = dataTable.Rows[i];

                                // Parse contract number
                                if (!long.TryParse(row["ContractNumber"]?.ToString(), out long contractNumber))
                                {
                                    model.ImportErrors.Add($"Row {i + 2}: Invalid Contract Number");
                                    model.ErrorCount++;
                                    continue;
                                }

                                // Parse ID submission date if present
                                DateOnly? idDateSubmittedOnly = null;
                                var idDateStr = row["IdDateSubmitted"]?.ToString();
                                if (!string.IsNullOrEmpty(idDateStr))
                                {
                                    if (DateTime.TryParseExact(idDateStr,
                                        new[] { "M/d/yy", "MM/dd/yy", "M/d/yyyy", "MM/dd/yyyy" },
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None,
                                        out DateTime parsedDate))
                                    {
                                        idDateSubmittedOnly = DateOnly.FromDateTime(parsedDate);
                                    }
                                }

                                var businessPartner = new BusinessPartner
                                {
                                    CustomerCode = row["CustomerCode"]?.ToString(),
                                    Role = row["Role"]?.ToString(),
                                    Fullname = row["Fullname"]?.ToString() ?? "",
                                    ClientBase = row["ClientBase"]?.ToString(),
                                    IdSubmitted = row["IdSubmitted"]?.ToString(),
                                    IdDateSubmitted = idDateSubmittedOnly,
                                    EmailAddress = row["EmailAddress"]?.ToString(),
                                    ContactNumber = row["ContactNumber"]?.ToString()
                                };

                                businessPartners.Add(businessPartner);
                                contractNumbers.Add(contractNumber);
                                model.SuccessCount++;
                            }
                            catch (Exception ex)
                            {
                                model.ImportErrors.Add($"Row {i + 2}: {ex.Message}");
                                model.ErrorCount++;
                            }
                        }

                        if (businessPartners.Any())
                        {
                            // Save business partners first to get their IDs
                            await _context.BusinessPartners.AddRangeAsync(businessPartners);
                            await _context.SaveChangesAsync();

                            // Only update SalesTransactions for Principal Buyers
                            for (int i = 0; i < businessPartners.Count; i++)
                            {
                                var partner = businessPartners[i];
                                var contractNumber = contractNumbers[i];

                                // Only update SalesTransaction if this is a Principal Buyer
                                if (partner.Role?.Trim().Equals("Principal Buyer", StringComparison.OrdinalIgnoreCase) == true)
                                {
                                    var transaction = await _context.SalesTransactions
                                        .FirstOrDefaultAsync(st => st.ContractNumber == contractNumber);

                                    if (transaction != null)
                                    {
                                        transaction.BusinessPartnerId = partner.BusinessPartnerId;
                                        await _context.SaveChangesAsync();
                                    }
                                }
                                else
                                {
                                    // For non-Principal Buyers (Spouse, Co-Buyer), verify if a Principal Buyer exists
                                    var transaction = await _context.SalesTransactions
                                        .Include(st => st.BusinessPartner)
                                        .FirstOrDefaultAsync(st => st.ContractNumber == contractNumber);

                                    if (transaction?.BusinessPartner != null)
                                    {
                                        // Update the CustomerCode to match the Principal Buyer's CustomerCode
                                        partner.CustomerCode = transaction.BusinessPartner.CustomerCode;
                                        await _context.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        model.ImportErrors.Add($"Row {i + 2}: No Principal Buyer found for Contract Number {contractNumber}. Please import Principal Buyer first.");
                                        model.ErrorCount++;
                                        model.SuccessCount--;
                                        // Remove the non-Principal Buyer since there's no Principal Buyer
                                        _context.BusinessPartners.Remove(partner);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }

                if (model.ErrorCount == 0 && model.SuccessCount > 0)
                {
                    TempData["SuccessMessage"] = $"Successfully imported {model.SuccessCount} business partners.";
                    return RedirectToAction("Index", "SalesTransaction");
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

        // GET: BusinessPartner/DownloadTemplate
        public IActionResult DownloadTemplate()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Chino");
            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Business Partner Import Template");

                // Add headers
                worksheet.Cells[1, 1].Value = "ContractNumber";
                worksheet.Cells[1, 2].Value = "CustomerCode";
                worksheet.Cells[1, 3].Value = "Role";
                worksheet.Cells[1, 4].Value = "Fullname";
                worksheet.Cells[1, 5].Value = "ClientBase";
                worksheet.Cells[1, 6].Value = "IdSubmitted";
                worksheet.Cells[1, 7].Value = "IdDateSubmitted";
                worksheet.Cells[1, 8].Value = "EmailAddress";
                worksheet.Cells[1, 9].Value = "ContactNumber";

                // Add sample data
                worksheet.Cells[2, 1].Value = "12345";
                worksheet.Cells[2, 2].Value = "CUST001";
                worksheet.Cells[2, 3].Value = "Principal Buyer";
                worksheet.Cells[2, 4].Value = "John Doe";
                worksheet.Cells[2, 5].Value = "Regular";
                worksheet.Cells[2, 6].Value = "Passport";
                worksheet.Cells[2, 7].Value = "5/21/2024";
                worksheet.Cells[2, 8].Value = "john.doe@email.com";
                worksheet.Cells[2, 9].Value = "09123456789";

                // Style the header row
                using (var range = worksheet.Cells[1, 1, 1, 9])
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
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BusinessPartnerImportTemplate.xlsx");
            }
        }
    }
} 
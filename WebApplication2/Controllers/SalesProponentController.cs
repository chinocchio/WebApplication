using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class SalesProponentController : Controller
    {
        private readonly DataContext _context;

        public SalesProponentController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Manage(long contractNumber)
        {
            var salesTransaction = await _context.SalesTransactions
                .Include(st => st.BusinessPartner)
                .FirstOrDefaultAsync(st => st.ContractNumber == contractNumber);

            if (salesTransaction == null)
            {
                return NotFound();
            }

            // Get all available proponents
            var availableProponents = await _context.SalesProponents
                .Select(sp => new SelectListItem
                {
                    Value = sp.ProponentBpNumber.ToString(),
                    Text = $"{sp.Roles} - {sp.Fullname} - {sp.ProponentBpNumber}"
                })
                .ToListAsync();

            // Get current proponent for this contract
            var currentProponent = await _context.SalesProponents
                .FirstOrDefaultAsync(sp => sp.ProponentBpNumber == salesTransaction.ProponentBpNumber);

            var currentProponents = new List<SalesProponent>();
            if (currentProponent != null)
            {
                currentProponents.Add(currentProponent);
            }

            var viewModel = new SalesProponentViewModel
            {
                ContractNumber = contractNumber,
                AvailableProponents = availableProponents,
                CurrentProponents = currentProponents
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProponent(
            long contractNumber,
            string team,
            // Team 1
            long? mdBpNumber, string? mdFullname,
            long? dmdBpNumber, string? dmdFullname, long? dmdReportingTo,
            long? mmBpNumber, string? mmFullname, long? mmReportingTo,
            long? moBpNumber, string? moFullname, long? moReportingTo,
            long? psBpNumber, string? psFullname, long? psReportingTo,
            long? brokerBpNumber, string? brokerFullname, long? brokerReportingTo,
            // Team 2
            long? dirBpNumber2, string? dirFullname2,
            long? brokerBpNumber2, string? brokerFullname2, long? brokerReportingTo2,
            long? smBpNumber2, string? smFullname2, long? smReportingTo2,
            long? agentBpNumber2, string? agentFullname2, long? agentReportingTo2,
            long? mpBpNumber2, string? mpFullname2, long? mpReportingTo2)
        {
            var salesTransaction = await _context.SalesTransactions
                .FirstOrDefaultAsync(st => st.ContractNumber == contractNumber);

            if (salesTransaction == null)
            {
                return Json(new { success = false, message = "Invalid contract number." });
            }

            async Task AddOrUpdateProponent(string role, long? bpNumber, string? fullName, long? reportingTo)
            {
                if (bpNumber.HasValue && !string.IsNullOrEmpty(fullName))
                {
                    var existingProponent = await _context.SalesProponents
                        .FirstOrDefaultAsync(sp => sp.ProponentBpNumber == bpNumber.Value);
                    if (existingProponent != null)
                    {
                        if (reportingTo.HasValue)
                        {
                            existingProponent.ReportingTo = reportingTo.Value.ToString();
                        }
                        if (!string.IsNullOrEmpty(fullName))
                        {
                            existingProponent.Fullname = fullName;
                        }
                        if (!string.IsNullOrEmpty(role))
                        {
                            existingProponent.Roles = role;
                        }
                    }
                    else
                    {
                        var newProponent = new SalesProponent
                        {
                            Roles = role,
                            ProponentBpNumber = bpNumber.Value,
                            Fullname = fullName,
                            ReportingTo = reportingTo.HasValue ? reportingTo.Value.ToString() : null
                        };
                        _context.SalesProponents.Add(newProponent);
                    }
                }
            }

            if (team == "team1")
            {
                await AddOrUpdateProponent("Marketing Director", mdBpNumber, mdFullname, null);
                await AddOrUpdateProponent("Deputy Marketing Director", dmdBpNumber, dmdFullname, dmdReportingTo);
                await AddOrUpdateProponent("Marketing Manager", mmBpNumber, mmFullname, mmReportingTo);
                await AddOrUpdateProponent("Marketing Officer", moBpNumber, moFullname, moReportingTo);
                await AddOrUpdateProponent("PS/QC/ISM", psBpNumber, psFullname, psReportingTo);
                await AddOrUpdateProponent("Broker", brokerBpNumber, brokerFullname, brokerReportingTo);
                await _context.SaveChangesAsync();
                // Set the lowest-level proponent's BP Number in SalesTransaction for Team 1
                if (brokerBpNumber.HasValue && !string.IsNullOrEmpty(brokerFullname))
                {
                    salesTransaction.ProponentBpNumber = brokerBpNumber.Value;
                }
                else if (psBpNumber.HasValue && !string.IsNullOrEmpty(psFullname))
                {
                    salesTransaction.ProponentBpNumber = psBpNumber.Value;
                }
                else if (moBpNumber.HasValue && !string.IsNullOrEmpty(moFullname))
                {
                    salesTransaction.ProponentBpNumber = moBpNumber.Value;
                }
                else if (mmBpNumber.HasValue && !string.IsNullOrEmpty(mmFullname))
                {
                    salesTransaction.ProponentBpNumber = mmBpNumber.Value;
                }
                else if (dmdBpNumber.HasValue && !string.IsNullOrEmpty(dmdFullname))
                {
                    salesTransaction.ProponentBpNumber = dmdBpNumber.Value;
                }
                else if (mdBpNumber.HasValue && !string.IsNullOrEmpty(mdFullname))
                {
                    salesTransaction.ProponentBpNumber = mdBpNumber.Value;
                }
            }
            else if (team == "team2")
            {
                await AddOrUpdateProponent("Director", dirBpNumber2, dirFullname2, null);
                await AddOrUpdateProponent("Broker", brokerBpNumber2, brokerFullname2, brokerReportingTo2);
                await AddOrUpdateProponent("Sales Manager", smBpNumber2, smFullname2, smReportingTo2);
                await AddOrUpdateProponent("Agent", agentBpNumber2, agentFullname2, agentReportingTo2);
                await AddOrUpdateProponent("Marketing Partner", mpBpNumber2, mpFullname2, mpReportingTo2);
                await _context.SaveChangesAsync();
                // Set the lowest-level proponent's BP Number in SalesTransaction for Team 2
                if (mpBpNumber2.HasValue && !string.IsNullOrEmpty(mpFullname2))
                {
                    salesTransaction.ProponentBpNumber = mpBpNumber2.Value;
                }
                else if (agentBpNumber2.HasValue && !string.IsNullOrEmpty(agentFullname2))
                {
                    salesTransaction.ProponentBpNumber = agentBpNumber2.Value;
                }
                else if (smBpNumber2.HasValue && !string.IsNullOrEmpty(smFullname2))
                {
                    salesTransaction.ProponentBpNumber = smBpNumber2.Value;
                }
                else if (brokerBpNumber2.HasValue && !string.IsNullOrEmpty(brokerFullname2))
                {
                    salesTransaction.ProponentBpNumber = brokerBpNumber2.Value;
                }
                else if (dirBpNumber2.HasValue && !string.IsNullOrEmpty(dirFullname2))
                {
                    salesTransaction.ProponentBpNumber = dirBpNumber2.Value;
                }
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Proponents added/updated successfully." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProponent(long contractNumber, long proponentBpNumber)
        {
            var salesTransaction = await _context.SalesTransactions
                .FirstOrDefaultAsync(st => st.ContractNumber == contractNumber);

            if (salesTransaction == null)
            {
                return NotFound();
            }

            // Remove the proponent from the sales transaction
            if (salesTransaction.ProponentBpNumber == proponentBpNumber)
            {
                salesTransaction.ProponentBpNumber = null;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Manage), new { contractNumber });
        }
    }
} 
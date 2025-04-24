using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class SalesTransactionController : Controller
    {
        private readonly DataContext _context;

        public SalesTransactionController(DataContext context)
        {
            _context = context;
        }

        // GET: SalesTransaction/Create
        public async Task<IActionResult> Create()
        {
            var model = new SalesTransactionCreateViewModel
            {
                AvailableBusinessPartners = await _context.BusinessPartners.ToListAsync(),
                AvailableProperties = await _context.Properties
                                        .Select(p => new Property
                                        {
                                            PropertyId = p.PropertyId,
                                            UnitCode = p.UnitCode,
                                            ProjectName = p.ProjectName,
                                            PropertyType = p.PropertyType,
                                            BuildingPhase = p.BuildingPhase,
                                            // computed display text for dropdown
                                            DisplayText = $"{p.UnitCode} - {p.ProjectName} / {p.PropertyType} / {p.BuildingPhase}"
                                        }).ToListAsync(),
                AvailableReservationFees = await _context.ReservationFees.ToListAsync(),
                AvailableSalesProponents = await _context.SalesProponents
                                        .Select(sp => new SalesProponent
                                        {
                                            SalesProponentId = sp.SalesProponentId,
                                            Broker = sp.Broker,
                                            PS_QC_ISM = sp.PS_QC_ISM,
                                            MarketingOfficer = sp.MarketingOfficer,
                                            MarketingManager = sp.MarketingManager,
                                            DeputyMarketingDirector = sp.DeputyMarketingDirector,
                                            MarketingDirector = sp.MarketingDirector,
                                            DisplayText = $"{sp.Broker} | {sp.PS_QC_ISM} | {sp.MarketingOfficer} | {sp.MarketingManager} | {sp.DeputyMarketingDirector} | {sp.MarketingDirector}"
                                        }).ToListAsync()
            };

            return View(model);
        }

        // POST: SalesTransaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesTransactionCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // Reload dropdown lists
                vm.AvailableBusinessPartners = await _context.BusinessPartners.ToListAsync();
                vm.AvailableProperties = await _context.Properties.ToListAsync();
                vm.AvailableReservationFees = await _context.ReservationFees.ToListAsync();
                vm.AvailableSalesProponents = await _context.SalesProponents.ToListAsync();
                return View(vm);
            }

            // Create new related entities if needed
            BusinessPartner? newPartner = null;
            if (!string.IsNullOrWhiteSpace(vm.NewBusinessPartnerFullName))
            {
                newPartner = new BusinessPartner
                {
                    Fullname = vm.NewBusinessPartnerFullName,
                    CustomerCode = vm.NewCustomerCode,
                    EmailAddress = vm.NewEmailAddress,
                    ContactNumber = vm.NewContactNumber
                };
                _context.BusinessPartners.Add(newPartner);
                await _context.SaveChangesAsync(); // Save to get ID
            }

            ReservationFee? newFee = null;
            if (vm.NewReservationFeeDatePaid.HasValue && vm.NewReservationFeeAmountPaid.HasValue)
            {
                newFee = new ReservationFee
                {
                    RfDatePaid = vm.NewReservationFeeDatePaid.Value,
                    RfAmountPaidToUnit = vm.NewReservationFeeAmountPaid.Value,
                    RfOrNumber = vm.NewReservationFeeOrNumber
                };
                _context.ReservationFees.Add(newFee);
                await _context.SaveChangesAsync();
            }

            // Enhanced logic: Reuse existing SalesProponent if matching full team already exists
            SalesProponent? newProponent = null;
            if (!string.IsNullOrWhiteSpace(vm.NewSalesProponentBroker))
            {
                // Check if a matching team already exists
                var existingTeam = await _context.SalesProponents.FirstOrDefaultAsync(sp =>
                    sp.Broker == vm.NewSalesProponentBroker &&
                    sp.PS_QC_ISM == vm.NewSalesProponentPSQCISM &&
                    sp.MarketingOfficer == vm.NewSalesProponentMarketingOfficer &&
                    sp.MarketingManager == vm.NewSalesProponentMarketingManager &&
                    sp.DeputyMarketingDirector == vm.NewSalesProponentDeputyMarketingDirector &&
                    sp.MarketingDirector == vm.NewSalesProponentMarketingDirector); // 👈 Check all fields

                if (existingTeam != null)
                {
                    newProponent = existingTeam; // 👈 Use existing if found
                }
                else
                {
                    newProponent = new SalesProponent // 👈 Else create new
                    {
                        Broker = vm.NewSalesProponentBroker,
                        PS_QC_ISM = vm.NewSalesProponentPSQCISM,
                        MarketingOfficer = vm.NewSalesProponentMarketingOfficer,
                        MarketingManager = vm.NewSalesProponentMarketingManager,
                        DeputyMarketingDirector = vm.NewSalesProponentDeputyMarketingDirector,
                        MarketingDirector = vm.NewSalesProponentMarketingDirector
                    };
                    _context.SalesProponents.Add(newProponent);
                    await _context.SaveChangesAsync(); // Save to get ID
                }
            }

            //SalesProponent? newProponent = null;
            //if (!string.IsNullOrWhiteSpace(vm.NewSalesProponentBroker))
            //{
            //    newProponent = new SalesProponent
            //    {
            //        Broker = vm.NewSalesProponentBroker,
            //        MarketingOfficer = vm.NewSalesProponentMarketingOfficer
            //    };
            //    _context.SalesProponents.Add(newProponent);
            //    await _context.SaveChangesAsync();
            //}

            var transaction = new SalesTransaction
            {
                ContractNumber = vm.ContractNumber,
                TypeOfSale = vm.TypeOfSale,
                HoldingDate = vm.HoldingDate,
                TransactionType = vm.TransactionType,
                PromoDiscount = vm.PromoDiscount,
                StatusInGeneral = vm.StatusInGeneral,
                Milestone = vm.Milestone,
                NewColorStatus = vm.NewColorStatus,
                BusinessPartnerId = newPartner?.BusinessPartnerId ?? vm.BusinessPartnerId,
                PropertyId = vm.PropertyId,
                ReservationFeeId = newFee?.ReservationFeeId ?? vm.ReservationFeeId,
                SalesProponentsId = newProponent?.SalesProponentId ?? vm.SalesProponentsId
            };

            _context.SalesTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

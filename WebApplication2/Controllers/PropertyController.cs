using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication2.Data;
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
            var query = _context.SalesTransactions.AsNoTracking()
                        .Include(st => st.Properties)
                        .Include(st => st.BusinessPartner)
                        .Include(st => st.SalesProponent)
                        .Include(st => st.ReservationFee)
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
             * Tapos para mas madali makita yung mga recent na transaction, nag order by tayo
             * sa HoldingDate in descending order
             */
            query = query.OrderByDescending(st => st.HoldingDate);

            /*
             * Tapos pasa nating sa PropertyListViewModel yung mga nakuha nating data
             * na gagamitin natin sa view
             */
            var model = new PropertyListViewModel
            {
                SalesTransactions = await query.ToListAsync(),
                SearchTerm = searchTerm
            };

            // Tapos papasa natin sa ModelView yung result nung controller na to
            return View(model);
        }
    }
}

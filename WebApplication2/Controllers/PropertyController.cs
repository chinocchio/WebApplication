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
            var query = _context.Properties.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(p => p.UnitCode.ToLower().Contains(searchTerm.ToLower()));

            var model = new PropertyListViewModel
            {
                Properties = await query.ToListAsync(),
                SearchTerm = searchTerm
            };

            return View(model);
        }
    }
}

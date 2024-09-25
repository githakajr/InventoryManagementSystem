using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;

namespace InventoryManagementSystem.Pages.Products
{
    public class CheckStockModel : PageModel
    {
        private readonly InventoryContext _context;

        public CheckStockModel(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var lowStockProducts = await _context.Products
                .Where(p => p.Quantity == 0)
                .Select(p => new { p.Id, p.Name })
                .ToListAsync();

            return new JsonResult(lowStockProducts);
        }
    }
}

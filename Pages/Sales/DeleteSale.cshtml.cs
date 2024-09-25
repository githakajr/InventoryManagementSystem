using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Pages.Sales
{
    public class DeleteSaleModel : PageModel
    {
        private readonly InventoryContext _context;

        public DeleteSaleModel(InventoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Sale? Sale { get; set; } = new Sale();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Sale = await _context.Sales
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Sale == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Sale == null)
            {
                return NotFound();
            }

            var saleToDelete = await _context.Sales.FindAsync(Sale.Id);

            if (saleToDelete == null)
            {
                return NotFound();
            }

            _context.Sales.Remove(saleToDelete);
            await _context.SaveChangesAsync();

            return RedirectToPage("./SalesHistory");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Pages.Sales
{
    public class EditSaleModel : PageModel
    {
        private readonly InventoryContext _context;

        public EditSaleModel(InventoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Sale? Sale { get; set; } = new Sale();

        public IList<Product> Products { get; set; } = new List<Product>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Sale = await _context.Sales
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Sale == null)
            {
                return NotFound();
            }

            Products = await _context.Products.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var saleToUpdate = await _context.Sales.FindAsync(Sale!.Id);
            if (saleToUpdate == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(Sale.ProductId);
            if (product == null)
            {
                ModelState.AddModelError(string.Empty, "Selected product not found.");
                return Page();
            }

            // Update sale details
            saleToUpdate.ProductId = Sale.ProductId;
            saleToUpdate.Quantity = Sale.Quantity;
            saleToUpdate.TotalAmount = Sale.Quantity * product.Price; // Safely access the price after null check

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(Sale.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./SalesHistory");
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }
    }
}

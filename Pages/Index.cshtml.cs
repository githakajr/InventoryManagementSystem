using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Pages
{
    public class IndexModel : PageModel
    {
        private readonly InventoryContext _context;

        public IndexModel(InventoryContext context)
        {
            _context = context;
        }

        // List of products that are out of stock
        public IList<Product> LowStockProducts { get; set; } = new List<Product>();

        public async Task OnGetAsync()
        {
            // Fetch products that are out of stock
            LowStockProducts = await _context.Products.Where(p => p.Quantity == 0).ToListAsync();
        }
    }
}

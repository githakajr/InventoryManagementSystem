using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly InventoryContext _context;

        public IndexModel(InventoryContext context)
        {
            _context = context;
            Products = new List<Product>();
        }

        public IList<Product> Products { get; set; }

        public async Task OnGetAsync()
        {
            Products = await _context.Products.Where(p => p.Quantity > 0).ToListAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;


namespace InventoryManagementSystem.Pages.Sales
{
    public class SalesHistoryModel : PageModel
    {
        private readonly InventoryContext _context;

        public SalesHistoryModel(InventoryContext context)
        {
            _context = context;
        }

        public IList<Sale> Sales { get; set; } = new List<Sale>();
         public string FormattedTotalSalesAmount { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            Sales = await _context.Sales.Include(s => s.Product).ToListAsync();
                // Calculate total sales amount and format it
            var totalSalesAmount = Sales.Sum(s => s.TotalAmount);
            FormattedTotalSalesAmount = totalSalesAmount.ToString("KSh #,0");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Pages.Sales
{
    public class IndexModel : PageModel
    {
        private readonly InventoryContext _context;

        public IndexModel(InventoryContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; } = new List<Product>();
        public IList<Sale> Sales { get; set; } = new List<Sale>();

        public IList<Product> LowStockProducts { get; set; } = new List<Product>();


        [BindProperty]
        public int ProductId { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public Sale Sale { get; set; } = new Sale(); // Ensure this is properly initialized
        public bool SaleSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty; // For error messages
    

public async Task OnGetAsync()
{
    // Only include products with quantity greater than 0
    Products = await _context.Products.Where(p => p.Quantity > 0).ToListAsync();
    
    // Retrieve all sales records for history display
    Sales = await _context.Sales.Include(s => s.Product).ToListAsync();

    // Check if any products have run out of stock
    LowStockProducts = await _context.Products.Where(p => p.Quantity == 0).ToListAsync();
}


        public async Task<IActionResult> OnPostAsync()
        {
            var product = await _context.Products.FindAsync(ProductId);

            if (product == null)
            {
                ErrorMessage = "Product not found.";
                SaleSuccess = false;
                return Page();
            }

            if (product.Quantity < Quantity)
            {
                ErrorMessage = "Insufficient quantity available.";
                SaleSuccess = false;
                return Page();
            }

            // Create and initialize the Sale object
            var sale = new Sale
            {
                ProductId = product.Id,
                Product = product, // Set Product to avoid null reference
                Quantity = Quantity,
                TotalAmount = Quantity * product.Price,
                Date = DateTime.Now
            };

            // Update product quantity
            product.Quantity -= Quantity;

            // Add sale record
            _context.Sales.Add(sale);

            // Use a transaction to ensure both operations succeed
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Reload sales history
                Products = await _context.Products.ToListAsync();
                Sales = await _context.Sales.Include(s => s.Product).ToListAsync();

                Sale = sale;
                SaleSuccess = true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ErrorMessage = $"An error occurred: {ex.Message}";
                SaleSuccess = false;
            }

            return Page();
        }
    }
}

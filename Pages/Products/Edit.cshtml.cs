using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Pages.Products
{
   public class EditModel : PageModel
{
    private readonly InventoryContext _context;

    public EditModel(InventoryContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Product? Product { get; set; } = new Product();

    public async Task<IActionResult> OnGetAsync(int id)
    {
#pragma warning disable CS8601 // Possible null reference assignment.
            Product = await _context.Products.FindAsync(id);
#pragma warning restore CS8601 // Possible null reference assignment.

            if (Product == null)
        {
            return NotFound(); // Handle null cases here
        }

        return Page();
    }

   public async Task<IActionResult> OnPostAsync(int id)
{
    if (!ModelState.IsValid)
    {
        return Page();
    }

    var productToUpdate = await _context.Products.FindAsync(id);

    if (productToUpdate == null)
    {
        return NotFound();
    }

    if (await TryUpdateModelAsync<Product>(
        productToUpdate,
        "product",
        p => p.Name, p => p.Description, p => p.Quantity, p => p.Price))
    {
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }

    return Page();
}
}
}

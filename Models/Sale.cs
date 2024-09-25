namespace InventoryManagementSystem.Models
{
public class Sale
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; } // Make Product nullable
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime Date { get; set; }
}


}
 
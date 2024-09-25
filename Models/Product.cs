namespace InventoryManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}

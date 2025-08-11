namespace Web.Models.Carts;

public class CartItemViewModel
{
    public Guid Id { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
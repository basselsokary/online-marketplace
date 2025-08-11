namespace Web.Models.Products;

public class EditProductViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public List<Guid> SelectedCategoryIds { get; set; } = null!;
    public string? ImageUrl { get; set; } = null!;
}


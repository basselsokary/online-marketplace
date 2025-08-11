using Domain.Enums;

namespace Web.Models.Orders;

public class EditOrderStatusViewModel
{
    public Guid OrderId { get; set; }
    public string Status { get; set; } = null!;
}

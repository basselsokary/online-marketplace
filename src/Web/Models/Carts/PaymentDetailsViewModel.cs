namespace Web.Models.Carts;

public class PaymentDetailsViewModel
{
    public string? CardNumber { get; set; }
    public string? CardHolderName { get; set; }
    public string? ExpiryDate { get; set; }
    public string? CVV { get; set; }
    public string? PayPalEmail { get; set; }
    public string? BankAccount { get; set; }
    public bool SaveCard { get; set; } = false;
}

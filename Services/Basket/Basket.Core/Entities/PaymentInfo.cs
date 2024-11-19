namespace Basket.Core.Entities;

public class PaymentInfo
{
    public string CardName { get; set; } = default!;
    public string CardNumber { get; set; } = default!;
    public string Expiration { get; set; } = default!;
    public string Cvv { get; set; } = default!;
    public int PaymentMethod { get; set; }
}

namespace Basket.Core.Entities;

public class BasketCheckout
{
    public UserInfo User { get; set; } = new UserInfo();
    public decimal TotalPrice { get; set; }
    public PaymentInfo Payment { get; set; } = new PaymentInfo();
}

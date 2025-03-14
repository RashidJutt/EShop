namespace EventBus.Events;

public class BasketCheckout : BaseIntegrationEvent
{
    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public Address Address { get; set; } = default!;
    public PaymentDetails PaymentDetails { get; set; } = default!;
}


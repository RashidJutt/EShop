namespace EventBus.Events;

public class BasketCheckoutV2 : BaseIntegrationEvent
{
    public string UserName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;
}

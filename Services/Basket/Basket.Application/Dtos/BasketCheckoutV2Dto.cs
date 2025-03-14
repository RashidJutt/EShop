namespace Basket.Application.Dtos;

public class BasketCheckoutV2Dto
{
    public string UserName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;
}

namespace Ordering.Application.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public AddressDto Address { get; set; } = default!;
    public PaymentDetailsDto PaymentDetails { get; set; } = default!;
}

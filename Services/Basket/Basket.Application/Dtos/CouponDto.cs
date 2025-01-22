namespace Discount.Application.Dtos;

public class CouponDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Amount { get; set; }
}

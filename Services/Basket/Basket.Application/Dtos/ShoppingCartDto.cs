using Basket.Core.Entities;

namespace Basket.Application.Dtos;

public class ShoppingCartDto
{
    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new();

}

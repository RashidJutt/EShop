using Basket.Application.Dtos;
using MediatR;

namespace Basket.Application.Commands;

public record UpdateShoppingCartCommand(ShoppingCartDto ShoppingCart) : IRequest<ShoppingCartDto>;
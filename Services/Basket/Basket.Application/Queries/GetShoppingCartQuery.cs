using Basket.Application.Dtos;
using MediatR;

namespace Basket.Application.Queries;

public record GetShoppingCartQuery(string UserName) : IRequest<ShoppingCartDto>;

using MediatR;

namespace Basket.Application.Commands;

public record DeleteShoppingCartCommand(string Username):IRequest;

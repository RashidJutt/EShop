using MediatR;

namespace Catalog.Application.Commands;

public record DeleteProductCommand(string Id) : IRequest<bool>;

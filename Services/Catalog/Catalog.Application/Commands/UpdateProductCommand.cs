using Catalog.Application.Dtos;
using MediatR;

namespace Catalog.Application.Commands;

public record UpdateProductCommand(ProductDto Product) : IRequest<bool>;
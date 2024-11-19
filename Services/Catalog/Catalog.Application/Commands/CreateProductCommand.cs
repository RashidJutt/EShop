using Catalog.Application.Dtos;
using MediatR;

namespace Catalog.Application.Commands;

public record CreateProductCommand(ProductDto Product):IRequest<ProductDto>;

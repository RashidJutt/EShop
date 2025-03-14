using Catalog.Application.Dtos;
using MediatR;

namespace Catalog.Application.Commands;

public record CreateProductCommand(CreateProductDto Product):IRequest<ProductDto>;

using Catalog.Application.Dtos;
using MediatR;

namespace Catalog.Application.Queries;

public record GetProductByIdQuery(string Id) : IRequest<ProductDto>;

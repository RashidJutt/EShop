using Catalog.Application.Dtos;
using MediatR;

namespace Catalog.Application.Queries;

public record GetProductsByNameQuery(string Name) : IRequest<IList<ProductDto>>;

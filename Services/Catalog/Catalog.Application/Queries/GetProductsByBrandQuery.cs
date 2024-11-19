using Catalog.Application.Dtos;
using MediatR;

namespace Catalog.Application.Queries;

public record GetProductsByBrandQuery(string BrandName):IRequest<IList<ProductDto>>;
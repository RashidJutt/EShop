using Catalog.Application.Dtos;
using Catalog.Core.Common;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Application.Queries;

public record GetAllProductsQuery(CatalogFilterParams Params) : IRequest<PagedResult<ProductDto>>;


using Catalog.Application.Dtos;
using MediatR;

namespace Catalog.Application.Queries;

public record GetAllBrandsQuery : IRequest<IList<BrandDto>>;

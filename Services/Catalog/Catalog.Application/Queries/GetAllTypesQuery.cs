using Catalog.Application.Dtos;
using MediatR;

namespace Catalog.Application.Queries;

public record GetAllTypesQuery : IRequest<IList<TypeDto>>;

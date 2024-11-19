using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Queries;

public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandDto>>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllBrandsHandler> _logger;

    public GetAllBrandsHandler(IBrandRepository brandRepository, IMapper mapper, ILogger<GetAllBrandsHandler> logger)
    {
        _brandRepository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IList<BrandDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _brandRepository.GetAllAsync(cancellationToken);

        if (brands == null || !brands.Any())
        {
            _logger.LogWarning("No brands found.");
            return new List<BrandDto>();
        }

        var brandDtos = _mapper.Map<IList<BrandDto>>(brands);
        return brandDtos;
    }
}

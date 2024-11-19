using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers;

public class GetProductsByBrandHandler : IRequestHandler<GetProductsByBrandQuery, IList<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductsByBrandHandler> _logger;

    public GetProductsByBrandHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProductsByBrandHandler> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IList<ProductDto>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching products for brand: {BrandName}", request.BrandName);

        var products = await _productRepository.GetByBrandAsync(request.BrandName, cancellationToken);

        if (products == null || !products.Any())
        {
            _logger.LogWarning("No products found for brand: {BrandName}", request.BrandName);
            return new List<ProductDto>();
        }

        var productDtos = _mapper.Map<IList<ProductDto>>(products);
        return productDtos;
    }
}

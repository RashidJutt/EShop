using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers;

public class GetProductsByNameHandler : IRequestHandler<GetProductsByNameQuery, IList<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductsByNameHandler> _logger;

    public GetProductsByNameHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProductsByNameHandler> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IList<ProductDto>> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching products with name: {ProductName}", request.Name);

        var products = await _productRepository.GetAllAsync(request.Name, cancellationToken);

        if (products == null || !products.Any())
        {
            _logger.LogWarning("No products found with name: {ProductName}", request.Name);
            return new List<ProductDto>();
        }

        var productDtos = _mapper.Map<IList<ProductDto>>(products);
        return productDtos;
    }
}

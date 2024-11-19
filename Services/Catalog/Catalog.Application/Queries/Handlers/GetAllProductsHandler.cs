using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Queries;
using Catalog.Core.Common;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, PagedResult<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductsHandler> _logger;

    public GetAllProductsHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetAllProductsHandler> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PagedResult<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(request.Params, cancellationToken);
        var productDtos = _mapper.Map<PagedResult<ProductDto>>(products);
        return productDtos;
    }
}

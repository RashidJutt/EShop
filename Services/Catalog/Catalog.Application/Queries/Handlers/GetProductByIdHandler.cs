using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Exceptions;
using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductByIdHandler> _logger;

    public GetProductByIdHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProductByIdHandler> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching product with ID: {ProductId}", request.Id);

        var product = await _productRepository.GetAsync(request.Id, cancellationToken);

        if (product == null)
        {
            _logger.LogWarning("Product with ID: {ProductId} was not found", request.Id);
            throw new NotFoundException($"Product with ID {request.Id} was not found.");
        }

        var productDto = _mapper.Map<ProductDto>(product);
        return productDto;
    }
}

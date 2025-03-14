using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Commands.Handlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper, ILogger<CreateProductCommandHandler> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new product with details: {@ProductRequest}", request);

        var productEntity = _mapper.Map<Product>(request.Product);
        var createdProduct = await _productRepository.CreateAsync(productEntity, cancellationToken);
               
        var productDto = _mapper.Map<ProductDto>(createdProduct);
        _logger.LogInformation("Product successfully created with ID: {ProductId}", productDto.Id);
        return productDto;
    }
}

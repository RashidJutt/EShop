using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper, ILogger<UpdateProductCommandHandler> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to update product with ID: {ProductId}", request.Product.Id);

        var productEntity = _mapper.Map<Product>(request.Product);
        var result = await _productRepository.UpdateAsync(productEntity, cancellationToken);

        if (result)
        {
            _logger.LogInformation("Product with ID: {ProductId} successfully updated", request.Product.Id);
        }
        else
        {
            _logger.LogWarning("Failed to update product with ID: {ProductId}. The product may not exist or an error occurred", request.Product.Id);
        }

        return result;
    }
}

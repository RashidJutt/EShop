using Catalog.Application.Commands;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Commands.Handlers;

public class DeleteProductByIDCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeleteProductByIDCommandHandler> _logger;

    public DeleteProductByIDCommandHandler(IProductRepository productRepository, ILogger<DeleteProductByIDCommandHandler> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to delete product with ID: {ProductId}", request.Id);

        var result = await _productRepository.DeleteAsync(request.Id, cancellationToken);

        if (result)
        {
            _logger.LogInformation("Product with ID: {ProductId} successfully deleted", request.Id);
        }
        else
        {
            _logger.LogWarning("Product with ID: {ProductId} could not be deleted or was not found", request.Id);
        }

        return result;
    }
}

using AutoMapper;
using Discount.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Discount.Application.Commands.Handlers;

public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommmand>
{
    private readonly ILogger<DeleteCouponCommandHandler> _logger;
    private readonly IDiscountRepository _discountRepository;

    public DeleteCouponCommandHandler(
        ILogger<DeleteCouponCommandHandler> logger,
        IDiscountRepository discountRepository)
    {
        _logger = logger;
        _discountRepository = discountRepository;
    }
    public async Task Handle(DeleteCouponCommmand command, CancellationToken cancellationToken)
    {
        var isDeleted = await _discountRepository.DeleteDiscountAsync(command.productName, cancellationToken);

        if (!isDeleted)
            throw new InvalidOperationException($"Failed to delete coupon for product {command.productName}");

        _logger.LogInformation("coupon successfully deleted for product {productName}", command.productName);
    }
}

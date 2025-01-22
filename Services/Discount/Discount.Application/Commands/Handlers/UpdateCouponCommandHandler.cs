using AutoMapper;
using Discount.Application.Dtos;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Discount.Application.Commands.Handlers;

public class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommand, CouponDto>
{
    private readonly ILogger<UpdateCouponCommandHandler> _logger;
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public UpdateCouponCommandHandler(
        ILogger<UpdateCouponCommandHandler> logger,
        IDiscountRepository discountRepository,
        IMapper mapper)
    {
        _logger = logger;
        _discountRepository = discountRepository;
        _mapper = mapper;
    }
    public async Task<CouponDto> Handle(UpdateCouponCommand command, CancellationToken cancellationToken)
    {
        var couponEntity = _mapper.Map<Coupon>(command.Coupon);
        var isUpdated = await _discountRepository.UpdateDiscountAsync(couponEntity, cancellationToken);

        if (!isUpdated)
            throw new InvalidOperationException($"Failed to update coupon for product {command.Coupon.ProductName}");


        _logger.LogInformation("Coupon successfully updated for product {productName}", command.Coupon.ProductName);
        return _mapper.Map<CouponDto>(couponEntity);
    }
}

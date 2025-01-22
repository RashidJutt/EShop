using AutoMapper;
using Discount.Application.Dtos;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Discount.Application.Commands.Handlers;

public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, CouponDto>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCouponCommandHandler> _logger;

    public CreateCouponCommandHandler(
        IDiscountRepository discountRepository,
        IMapper mapper,
        ILogger<CreateCouponCommandHandler> logger)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<CouponDto> Handle(CreateCouponCommand command, CancellationToken cancellationToken)
    {
        var couponEntity = _mapper.Map<Coupon>(command.Request);
        var isCreated = await _discountRepository.CreateDiscountAsync(couponEntity, cancellationToken);

        if (!isCreated)
            throw new InvalidOperationException($"Failed to create coupon for product {couponEntity.ProductName}.");

        _logger.LogInformation("Coupon successfully created for product {ProductName}.", couponEntity.ProductName);
        return _mapper.Map<CouponDto>(couponEntity);
    }
}

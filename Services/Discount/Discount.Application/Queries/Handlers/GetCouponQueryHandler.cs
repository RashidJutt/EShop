using AutoMapper;
using Discount.Application.Dtos;
using Discount.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Discount.Application.Queries.Handlers;

public class GetCouponQueryHandler : IRequestHandler<GetCouponQuery, CouponDto?>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly ILogger<GetCouponQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetCouponQueryHandler(
        IDiscountRepository discountRepository,
        ILogger<GetCouponQueryHandler> logger,
        IMapper mapper)
    {
        _discountRepository = discountRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CouponDto?> Handle(GetCouponQuery command, CancellationToken cancellationToken)
    {
        var couponEntity = await _discountRepository.GetDiscountAsync(command.ProductName, cancellationToken);

        if (couponEntity == null)
        {
            _logger.LogInformation("Coupon not found for product {ProductName}.", command.ProductName);
            return new CouponDto();
        }

        _logger.LogInformation("Coupon found for product {ProductName}.", command.ProductName);
        return _mapper.Map<CouponDto>(couponEntity);
    }
}

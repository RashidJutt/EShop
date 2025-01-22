using Discount.Application.Commands;
using Discount.Application.Dtos;
using Discount.Application.Queries;
using MediatR;

namespace Discount.API.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IMediator _mediator;

        public DiscountService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CouponDto?> GetDiscount(CouponRequestDto couponRequest)
        {
            return await _mediator.Send(new GetCouponQuery(couponRequest.ProductName));
        }

        public async Task<CouponDto> CreateDiscount(CouponDto coupon)
        {
            return await _mediator.Send(new CreateCouponCommand(coupon));
        }

        public async Task<CouponDto> UpdateDiscount(CouponDto coupon)
        {
            return await _mediator.Send(new UpdateCouponCommand(coupon));
        }

        public async Task<DeleteResultDto> DeleteDiscount(CouponRequestDto deleteRequst)
        {
            await _mediator.Send(new DeleteCouponCommmand(deleteRequst.ProductName));
            return new DeleteResultDto();
        }
    }
}

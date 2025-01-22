using Discount.Application.Dtos;
using MediatR;

namespace Discount.Application.Commands;

public record UpdateCouponCommand(CouponDto Coupon) : IRequest<CouponDto>;

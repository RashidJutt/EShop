using Discount.Application.Dtos;
using MediatR;

namespace Discount.Application.Commands;

public record CreateCouponCommand(CouponDto Request) : IRequest<CouponDto>;

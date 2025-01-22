using Discount.Application.Dtos;
using MediatR;

namespace Discount.Application.Queries;

public record GetCouponQuery(string ProductName) : IRequest<CouponDto?>;

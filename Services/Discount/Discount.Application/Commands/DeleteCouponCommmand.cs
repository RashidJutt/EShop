using MediatR;

namespace Discount.Application.Commands;

public record DeleteCouponCommmand(string productName) : IRequest;

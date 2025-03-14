using Ordering.Core.Entities;

namespace Ordering.Application.Services;

public interface IOrderService
{
    Task CreateOrderAsync(Order order, CancellationToken cancellationToken);
}

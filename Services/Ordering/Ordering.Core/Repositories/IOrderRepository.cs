using Ordering.Core.Entities;
using System.Linq.Expressions;

namespace Ordering.Core.Repositories;

public interface IOrderRepository
{
    Task<IReadOnlyList<Order>> GetAllAsync(Expression<Func<Order, bool>> predicate);
    Task<Order?> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(Order order);
}

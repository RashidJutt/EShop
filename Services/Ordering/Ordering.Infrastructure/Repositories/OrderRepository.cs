using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System.Linq.Expressions;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .FindAsync(id);
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(Expression<Func<Order, bool>> predicate)
    {
        return await _context.Orders
            .Where(predicate)
            .ToListAsync();
    }

    public Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Order order)
    {
        _context.Orders.Remove(order);
        return Task.CompletedTask;
    }
}

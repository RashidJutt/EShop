using System.Data;
using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;

namespace Discount.Infrastructure.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly DataContext _context;
    private readonly ILogger<DiscountRepository> _logger;

    public DiscountRepository(DataContext context, ILogger<DiscountRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Coupon?> GetDiscountAsync(string productName, CancellationToken cancellationToken)
    {
        try
        {
            var sql = """
                SELECT * FROM Coupons
                WHERE ProductName = @productName
                LIMIT 1
            """;

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Coupon>(sql, new { productName });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error is fetching discount for product {productName}.", productName);
            throw new DataException($"Error fetching discount for product '{productName}'.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> CreateDiscountAsync(Coupon coupon, CancellationToken cancellationToken)
    {
        try
        {
            var sql = """
                INSERT INTO Coupons (ProductName, Description, Amount)
                VALUES (@ProductName, @Description, @Amount)
            """;

            using var connection = _context.CreateConnection();
            var effectedRows = await connection.ExecuteAsync(sql, new
            {
                coupon.ProductName,
                coupon.Description,
                coupon.Amount
            });

            return effectedRows > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating discount for product {productName}.", coupon.ProductName);
            throw new DataException($"Error creating discount for product '{coupon.ProductName}'.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateDiscountAsync(Coupon coupon, CancellationToken cancellationToken)
    {
        try
        {
            var sql = """
                UPDATE Coupons
                SET ProductName = @ProductName, Description = @Description, Amount = @Amount
                WHERE Id = @Id
            """;

            using var connection = _context.CreateConnection();
            var effectedRows = await connection.ExecuteAsync(sql, new
            {
                coupon.ProductName,
                coupon.Description,
                coupon.Amount,
                coupon.Id
            });

            return effectedRows > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating discount for product {ProductName}.", coupon.ProductName);
            throw new DataException($"Error updating discount for product '{coupon.ProductName}'.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteDiscountAsync(string productName, CancellationToken cancellationToken)
    {
        try
        {
            var sql = """
                DELETE FROM Coupons
                WHERE ProductName = @ProductName
            """;

            using var connection = _context.CreateConnection();
            var effectedRows = await connection.ExecuteAsync(sql, new { productName });

            return effectedRows > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting discount for product {productName} .", productName);
            throw new DataException($"Error deleting discount for product '{productName}'.", ex);
        }
    }
}

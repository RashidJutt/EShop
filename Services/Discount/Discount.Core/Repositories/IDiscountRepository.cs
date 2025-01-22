using System.Threading.Tasks;
using Discount.Core.Entities;

namespace Discount.Core.Repositories;

/// <summary>
/// Defines the repository interface for managing discounts.
/// </summary>
public interface IDiscountRepository
{
    /// <summary>
    /// Retrieves a discount for the specified product.
    /// </summary>
    /// <param name="productName">The name of the product.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="Coupon"/> associated with the product.</returns>
    Task<Coupon?> GetDiscountAsync(string productName, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new discount.
    /// </summary>
    /// <param name="coupon">The discount coupon to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> if the creation is successful; otherwise, <c>false</c>.</returns>
    Task<bool> CreateDiscountAsync(Coupon coupon, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing discount.
    /// </summary>
    /// <param name="coupon">The discount coupon to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> if the update is successful; otherwise, <c>false</c>.</returns>
    Task<bool> UpdateDiscountAsync(Coupon coupon, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the discount associated with the specified product.
    /// </summary>
    /// <param name="productName">The name of the product.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> if the deletion is successful; otherwise, <c>false</c>.</returns>
    Task<bool> DeleteDiscountAsync(string productName, CancellationToken cancellationToken);
}
